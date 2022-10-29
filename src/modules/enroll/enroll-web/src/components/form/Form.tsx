import React, { useState, useContext } from 'react'
import { Form, FormProps as AntdFormProps, message } from 'antd'
import FormContext from './FormContext'
import useSubmit from '../../utils/useSubmit'
import { OverlayContext } from '../overlay'
const labelLayout = {
  labelCol: { span: 4 },
  wrapperCol: { span: 18 },
}
type FormApi<D, Return, Params> = (path: Params, data: D) => Promise<Return>
function isFormApi<D, Return, Params>(
  api: FormApi<D, Return, Params> | Api<D>,
): api is FormApi<D, Return, Params> {
  return api.length === 2
}
export interface FormProps<D, P, R> extends AntdFormProps {
  api?: Api<D, R>
  params?: P
  canDismiss?: (data: R) => boolean
  labels?: { [key in string]: string }
  noLabel?: boolean
  viewType?: ViewType
}

// type FormProps = Parameters<Form>[0]

function ApiForm<D, Params = any, Return = any>(props: FormProps<D, Params, Return>) {
  const { noLabel, api, params, labels, ...rest } = props
  let [form] = Form.useForm()
  let overlay = useContext(OverlayContext)
  const [loading, setLoading] = useState(false)
  form = props.form ?? form
  //消除loading的警告
  const onFinish = useSubmit(async (values: any) => {
    try {
      values = { ...props.initialValues, ...values }
      setLoading(true)
      overlay.setLoading(true)
      let formData = { ...props, ...values }
      // 通过onBefore 返回false 阻止发送请求

      formData = await onBefore(formData)
      if (formData === false) {
        return
      }

      let data
      if (params) {
        data = await api(params!, formData)
      } else {
        data = await api(formData)
      }

      await props.onSuccess?.(data)
    } catch (ex: any) {
      console.error('onFail', ex)
      // if (!props.onFail && ex?.data?.msg) message.error(ex?.data?.msg, 4)
    } finally {
      setLoading(false)
      overlay.setLoading && overlay.setLoading(false)
    }
  })

  const onFinishFailed = (errorInfo: any) => {
    console.log('onFinishFailed:', errorInfo)
  }
  const layout = noLabel ? null : labelLayout

  return (
    <FormContext.Provider value={{ form, loading, setLoading, noLabel, labels }}>
      <Form
        form={form}
        autoComplete="off"
        layout="horizontal"
        colon={true}
        {...layout}
        {...rest}
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      />
    </FormContext.Provider>
  )
}
ApiForm.useForm = Form.useForm
ApiForm.List = Form.List
ApiForm.Item = Form.Item
export default ApiForm
