import withFormItem, { FormItemProps } from './withFormItem'
import { Input, InputProps, InputNumber, InputNumberProps } from 'antd'
import { TextAreaProps } from 'antd/es/input'

export const FormInputNumber = withFormItem(InputNumber)

export const FormTextArea = withFormItem(Input.TextArea)

export const FormInput = withFormItem(Input)
export type FormItemInput = InputProps & FormItemProps & { control: 'input' }
export type FormItemTextArea = TextAreaProps & FormItemProps & { control: 'textarea' }
export type FormItemInputNumber = InputNumberProps & FormItemProps & { control: 'inputNumber' }
FormInputNumber.defaulItemProps = {
  placeholder: '请输入',
}

FormInput.defaulItemProps = {
  placeholder: '请输入',
}
FormTextArea.defaulItemProps = {
  placeholder: '请输入',
}
export default FormInput
