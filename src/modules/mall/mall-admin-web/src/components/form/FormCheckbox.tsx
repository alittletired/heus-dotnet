import { Checkbox } from 'antd'
import withFormItem from './withFormItem'
const FormCheckbox = withFormItem(Checkbox)
FormCheckbox.defaultProps = {
  noStyle: true,
  valuePropName: 'checked',
}

export default FormCheckbox
