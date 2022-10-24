import React, {useMemo} from 'react'
import {Radio} from 'antd'
import {RadioGroupProps as AntdRadioGroupProps} from 'antd/lib/radio'
import withFormItem from './withFormItem'
import {normalizeOptions, OptionsType} from '../select'
import {useOptions} from '../select/OptionUtils'
type RadioGroupProps = Omit<AntdRadioGroupProps, 'options'> & {
  radioType?: 'radio' | 'button'
  options: OptionsType
}
// 自定义组件需要接收ref，否则会报警告
const FormRadioGroup = withFormItem((props: RadioGroupProps) => {
  let {radioType = 'radio', options, ...rest} = props
  let radioOptions = useOptions(props.options)
  const optionsDom = useMemo(() => {
    return radioOptions.map((option, index) => {
      let {value, label} = option
      if (radioType === 'button')
        return (
          <Radio.Button key={index} value={value}>
            {label}
          </Radio.Button>
        )

      return (
        <Radio key={index} value={value}>
          {label}
        </Radio>
      )
    })
  }, [radioType, radioOptions])
  return <Radio.Group {...rest}>{optionsDom}</Radio.Group>
})
FormRadioGroup.defaulItemProps = {placeholder: '请选择'}
export default FormRadioGroup
