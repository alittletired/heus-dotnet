import { Checkbox } from 'antd'
import withFormItem from './withFormItem'
const FormCheckbox = withFormItem(Checkbox)
FormCheckbox.defaulItemProps = {
  valuePropName: 'checked',
  noStyle: true,
  placeholder: '请选择',
}
export default FormCheckbox
