import { Checkbox, CheckboxProps } from 'antd'
import withFormItem from './withFormItem'
export type FormItemCheckbox = CheckboxProps & { control: 'checkbox' }
const FormCheckbox = withFormItem(Checkbox)
FormCheckbox.defaulItemProps = {
  valuePropName: 'checked',
  noStyle: true,
  placeholder: '请选择',
}
export default FormCheckbox
