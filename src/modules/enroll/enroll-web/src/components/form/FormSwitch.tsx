import { Switch, SwitchProps } from 'antd'
import withFormItem, { FormItemProps } from './withFormItem'
export type FormItemSwitch = SwitchProps & FormItemProps & { control: 'switch' }
const FormSwitch = withFormItem(Switch)
FormSwitch.defaulItemProps = { valuePropName: 'checked' }
export default FormSwitch
