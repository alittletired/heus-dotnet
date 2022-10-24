import React, {useContext} from 'react'
import {ButtonProps} from 'antd/es/button'
import {Button} from 'antd'
import withFormItem from './withFormItem'
import FormContext from './FormContext'
type FormButtonProps = ButtonProps & {
  title: string
}

const FormButton = withFormItem((props: FormButtonProps) => {
  let {title, ...restProps} = props
  return <Button {...restProps}>{title}</Button>
})
export default FormButton

export const SubmitButton = withFormItem((props: FormButtonProps) => {
  const formContext = React.useContext(FormContext)
  return (
    <FormButton
      type="primary"
      htmlType="submit"
      loading={formContext.loading}
      {...props}
    />
  )
})
