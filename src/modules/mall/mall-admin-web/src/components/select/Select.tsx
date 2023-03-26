import React from 'react'
import {Select as AntdSelect} from 'antd'
import {SelectProps, useOptions} from './OptionUtils'

export default function Select(props: SelectProps) {
  let {hasAllOption, defaultValue = null, ...rest} = props
  let style = props.style || {minWidth: 120}
  let options = useOptions(props.options)
  if (hasAllOption) {
    options.unshift({label: '全部', value: null})
  }
  return (
    <AntdSelect style={style} defaultValue={defaultValue} {...rest} options={options} />
  )
}
