import withFormItem from './withFormItem'
import { Input, InputNumber } from 'antd'

export const FormInputNumber = withFormItem(InputNumber)
export const FormTextArea = withFormItem(Input.TextArea)
export const FormInput = withFormItem(Input)

FormInputNumber.defaultProps = {
  placeholder: '请输入',
}

FormInput.defaultProps = {
  placeholder: '请输入',
}
FormTextArea.defaultProps = {
  placeholder: '请输入',
}
export default FormInput
