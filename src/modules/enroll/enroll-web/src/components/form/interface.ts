import { Cascader, Checkbox, Form, FormProps as AntdFormProps, Input, InputNumber } from 'antd'
import React from 'react'
import FormButton, { SubmitButton } from './FormButton'
// import { FormItemButton } from './FormButton'
// import { FormItemCascader } from './FormCascader'
// import { FormItemCheckbox } from './FormCheckbox'
// import { FormItemCheckboxGroup } from './FormCheckGroup'
// import { FormItemInput, FormItemInputNumber, FormItemTextArea } from './FormInput'
// import { FormItemTreeSelect } from './FormTreeSelect'
// import { FormItemSelect } from './FormSelect'
interface FormControlOption<P> {
  type: string
  control: React.ComponentType<P>
}
export const formControls = {
  button: FormButton,
  submitButton: SubmitButton,
  cascader: Cascader,
  checkbox: Checkbox,
  checkboxGroup: Checkbox.Group,
  input: Input,
  textarea: Input.TextArea,
  inputNumber: InputNumber,
}
export interface FormProps<D, P, R> extends AntdFormProps, ApiProps<D, P, R> {
  params?: P
  canDismiss?: (data: R) => boolean
  labels?: { [key in string]: string }
  noLabel?: boolean
  viewType?: ViewType
  // items?: FormItemOption[]
}
//   export type FormItemOption =
//   | FormItemButton
//   | FormItemCascader
//   | FormItemCheckbox
//   | FormItemCheckboxGroup
//   | FormItemInput
//   | FormItemTextArea
//   | FormItemInputNumber
//   | FormItemTreeSelect
//   | FormItemSelect
