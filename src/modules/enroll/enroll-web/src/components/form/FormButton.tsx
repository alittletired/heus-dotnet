import React, { useContext } from 'react'
import { Button, ButtonProps as AntdButtonProps } from 'antd'
import withFormItem, { FormItemProps } from './withFormItem'
import FormContext from './FormContext'
export type FormButtonProps = AntdButtonProps & {
  title: string
}
export type FormItemButton = { control: 'button' } & FormButtonProps & FormItemProps
const FormButton = withFormItem((props: FormButtonProps) => {
  let { title, ...restProps } = props
  return <Button {...restProps}>{title}</Button>
})
export default FormButton

export const SubmitButton = withFormItem((props: FormButtonProps) => {
  const formContext = React.useContext(FormContext)
  return <FormButton type="primary" htmlType="submit" loading={formContext.loading} {...props} />
})
