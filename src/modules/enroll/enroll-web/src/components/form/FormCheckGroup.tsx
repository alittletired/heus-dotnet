import React, { useMemo, Ref } from 'react'
import { Checkbox } from 'antd'
import withFormItem, { FormItemProps } from './withFormItem'
import { normalizeOptions, OptionType } from '../select'
import { CheckboxGroupProps } from 'antd/es/checkbox'
export type FormItemCheckboxGroup = CheckboxGroupProps &
  FormItemProps & { control: 'checkboxGroup' }
interface Props extends Omit<CheckboxGroupProps, 'options'> {
  options?: OptionType[]
}

const FormCheckGroup = withFormItem((props: Props) => {
  const checkOptions = useMemo(() => normalizeOptions(props.options), [props.options])

  return <Checkbox.Group {...props} options={checkOptions} />
})
FormCheckGroup.defaulItemProps = { placeholder: '请选择' }
export default FormCheckGroup
