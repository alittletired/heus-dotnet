import React, { useMemo } from 'react'
import { Checkbox } from 'antd'
import withFormItem from './withFormItem'
import { normalizeOptions, OptionType } from '../select'
import { CheckboxGroupProps } from 'antd/es/checkbox'

interface Props extends Omit<CheckboxGroupProps, 'options'> {
  options?: OptionType[]
}

const FormCheckGroup = withFormItem((props: Props) => {
  const options = useMemo(() => normalizeOptions(props.options), [props.options])

  return <Checkbox.Group {...props} options={options} />
})
// FormCheckGroup.defaulItemProps = { placeholder: '请选择' }
export default FormCheckGroup
