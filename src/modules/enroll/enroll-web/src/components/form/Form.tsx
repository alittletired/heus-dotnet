import React, { useState, useContext, useMemo } from 'react'
import { Form, FormProps as AntdFormProps, message } from 'antd'
import FormContext from './FormContext'
import useSubmit from '../../utils/useSubmit'
import { formControls } from './FormItem'

const labelLayout = {
  labelCol: { span: 4 },
  wrapperCol: { span: 18 },
}
function isParamApi<D, P, R>(api?: ParamApi<D, P, R> | Api<D, R>): api is ParamApi<D, P, R> {
  return api?.length === 2
}
type ValueOf<T> = T[keyof T]
type FormControls = typeof formControls
type FormControlKeys = keyof FormControls
type ComponentProps<T, D> = T extends React.ComponentType<infer P>
  ? ComponentPropsWithName<P, D>
  : never
type ComponentPropsWithName<P, D> = P extends { name: any } ? P & { name: keyof D } : P
type FormControlProps<D> = {
  [key in FormControlKeys]: { control: key } & ComponentProps<FormControls[key], D>
}
export type FormControlItem<D> = ValueOf<FormControlProps<D>>

export interface FormProps<D, P, R> extends AntdFormProps, ApiProps<D, P, R> {
  params?: P

  canDismiss?: (data: R) => boolean
  labels?: { [key in string]: string }
  noLabel?: boolean
  viewType?: ViewType
  items?: FormControlItem<D>[]
  initialValues: D
  children?: React.ReactNode
}

// type FormProps = Parameters<Form>[0]

function ApiForm<D, Params = any, Return = any>(props: FormProps<D, Params, Return>) {
  const { noLabel, api, onBefore, onSuccess, params, labels, children, items, ...rest } = props
  let [form] = Form.useForm()
  const [loading, setLoading] = useState(false)
  form = props.form ?? form
  const onFinish = useSubmit(async (values: any) => {
    try {
      let formData = { ...props.initialValues, ...values }
      setLoading(true)

      // 通过onBefore 返回false 阻止发送请求
      if (onBefore) formData = await onBefore?.(formData)
      if (values === false) {
        return
      }
      if (api) {
        let data
        if (isParamApi(api)) {
          data = await api(params!, formData)
        } else {
          data = await api(formData)
        }
        await onSuccess?.(data)
      }
    } catch (ex: any) {
      console.error('onFail', ex)
      // if (!props.onFail && ex?.data?.msg) message.error(ex?.data?.msg, 4)
    } finally {
      setLoading(false)
    }
  })

  const onFinishFailed = (errorInfo: any) => {
    console.log('onFinishFailed:', errorInfo)
  }
  const layout = noLabel ? null : labelLayout
  const itemDom = items?.map((item, idx) => {
    let { control, ...rest } = item
    const FormControlItem = formControls[control]
    const key = 'name' in rest ? String(rest['name']) : `formitem_${idx}`
    return <FormControlItem {...(rest as any)} key={key} />
  })
  return (
    <FormContext.Provider value={{ form, loading, setLoading, noLabel }}>
      <Form
        form={form}
        autoComplete="off"
        layout="horizontal"
        colon={true}
        {...layout}
        {...rest}
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}>
        {itemDom}
        {children}
      </Form>
    </FormContext.Provider>
  )
}
ApiForm.useForm = Form.useForm
ApiForm.List = Form.List
ApiForm.Item = Form.Item
export default ApiForm
