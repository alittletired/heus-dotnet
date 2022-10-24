import React, {useMemo, Ref} from 'react'
import {Checkbox} from 'antd'
import {CheckboxGroupProps, CheckboxOptionType} from 'antd/lib/checkbox'
import withFormItem from './withFormItem'
import {normalizeOptions, OptionType} from '../select'
interface Props extends Omit<CheckboxGroupProps, 'options'> {
  options?: OptionType[]
}

const FormCheckGroup = withFormItem((props: Props) => {
  const checkOptions = useMemo(() => normalizeOptions(props.options), [props.options])

  return <Checkbox.Group {...props} options={checkOptions} />
})
FormCheckGroup.defaulItemProps = {placeholder: '请选择'}
export default FormCheckGroup
