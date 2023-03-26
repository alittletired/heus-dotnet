import React from 'react'
import { Button, ButtonProps as AntdButtonProps } from 'antd'
import withFormItem from './withFormItem'
import FormContext from './FormContext'
export type FormButtonProps = AntdButtonProps & {
  title: string
}
const FormButton = withFormItem((props: FormButtonProps) => {
  return <Button {...props}>{props.children && props.title}</Button>
})
export default FormButton

export const SubmitButton = withFormItem((props: FormButtonProps) => {
  const formContext = React.useContext(FormContext)
  return (
    <Button loading={formContext.loading} {...props}>
      {props.children ?? props.title}
    </Button>
  )
})
SubmitButton.defaultProps = {
  type: 'primary',
  htmlType: 'submit',
}
