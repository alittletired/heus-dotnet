import { Checkbox } from 'antd'
import withFormItem from './withFormItem'
const FormCheckbox = withFormItem(Checkbox)
FormCheckbox.defaulItemProps = {
  noStyle: true,
}
FormCheckbox.defaultControlProps = {
  // placeholder: '请选择',
  //  valuePropName: 'checked'
}
export default FormCheckbox
