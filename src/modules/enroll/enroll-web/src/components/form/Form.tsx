import React, { useState, useContext, useMemo } from 'react'
import { Form, FormProps as AntdFormProps, message } from 'antd'
import FormContext from './FormContext'
import useSubmit from '../../utils/useSubmit'
import { formControls } from './FormItem'
import { OverlayContext } from '../overlay'

const labelLayout = {
  labelCol: { span: 4 },
  wrapperCol: { span: 18 },
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

export type FormProps<D, T extends (...args: any) => any> = Omit<
  AntdFormProps<D>,
  'initialValues'
> &
  ApiRequest<T> & {
    labels?: { [key in string]: string }
    noLabel?: boolean
    viewType?: ViewType
    initialValues: ApiDataType<T>
    items?: FormControlItem<ApiDataType<T>>[]
    children?: React.ReactNode
  }

// type FormProps = Parameters<Form>[0]

function ApiForm<D, A extends (...args: any) => any>(props: FormProps<D, A>) {
  const { noLabel, request, onBefore, onSuccess, labels, children, items, ...rest } = props
  let [form] = Form.useForm()
  const [loading, setLoading] = useState(false)
  let overlay = useContext(OverlayContext)
  form = props.form ?? form
  const onFinish = useSubmit(async (values: any) => {
    try {
      if (!request) return
      let formData = { ...props.initialValues, ...values }
      setLoading(true)
      overlay.setLoading(true)
      // 通过onBefore 返回false 阻止发送请求
      if (onBefore) formData = await onBefore?.(formData)
      if (values === false) {
        return
      }

      let data: ReturnType<A>
      if ('params' in props) {
        data = await request(props['params'], formData)
      } else {
        data = await request(formData)
      }
      await onSuccess?.(data)
      overlay.onDismiss(data)
    } catch (ex: any) {
      console.error('onFail', ex)
      // if (!props.onFail && ex?.data?.msg) message.error(ex?.data?.msg, 4)
    } finally {
      setLoading(false)
      overlay.setLoading(false)
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
