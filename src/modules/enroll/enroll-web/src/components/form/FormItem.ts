import withFormItem from './withFormItem'
import FormText from './FormText'
import Checkbox from './FormCheckbox'
import {
  FormInput as Input,
  FormTextArea as TextArea,
  FormInputNumber as InputNumber,
} from './FormInput'
import Select from './FormSelect'
import Text from './FormText'
import Switch from './FormSwitch'
import RadioGroup from './FormRadioGroup'
import CheckboxGroup from './FormCheckGroup'
import VerifyCode from './FormVerifyCode'
import Button, { SubmitButton } from './FormButton'
import Avatar from './FormAvatar'
import Cascader from './FormCascader'
import TreeSelect from './FormTreeSelect'
import {
  FormDatePicker as DatePicker,
  FormWeekPicker as WeekPicker,
  FormMonthPicker as MonthPicker,
  FormRangePicker as RangePicker,
} from './FormDatePicker'

export const formControls = {
  button: Button,
  submitButton: SubmitButton,
  cascader: Cascader,
  checkbox: Checkbox,
  checkboxGroup: CheckboxGroup,
  input: Input,
  textarea: TextArea,
  inputNumber: InputNumber,
  datePicker: DatePicker,
  weekPicker: WeekPicker,
  monthPicker: MonthPicker,
  select: Select,
  text: FormText,
}
export const Item = withFormItem((props: React.PropsWithChildren<any>) => {
  return props.children
})
export { withFormItem }
const FormItem = {
  Item,
  Checkbox,
  Input,
  TextArea,
  InputNumber,
  Select,
  Text,
  Switch,
  RadioGroup,
  Button,
  SubmitButton,
  CheckboxGroup,
  DatePicker,
  WeekPicker,
  MonthPicker,
  RangePicker,
  TreeSelect,
  Avatar,
  Cascader,
  VerifyCode,
}
export default FormItem
