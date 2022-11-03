import { Checkbox } from 'antd'
import withFormItem from './withFormItem'
const FormCheckbox = withFormItem(Checkbox)
FormCheckbox.defaulItemProps = {
  noStyle: true,
  valuePropName: 'checked',
}
FormCheckbox.defaultControlProps = {
  // placeholder: '请选择',
}
export default FormCheckbox
