import React, {ComponentClass} from 'react'
import withFormItem from './withFormItem'
import {TimePicker as AntdTimePicker, DatePicker as AntdDatePicker} from 'antd'
import moment, {Moment} from 'moment'
import 'moment/locale/zh-cn'
moment.locale('zh-cn')
const dateFormat = 'YYYY-MM-DD'
const monthFormat = 'YYYY-MM'
const timeFormat = 'HH:mm:ss'

function normalize(format: string, value: any): any {
  if (!value) return value
  if (Array.isArray(value)) {
    return value.map((v) => normalize(format, v))
  }
  if (typeof value === 'string') {
    if (value.includes(',')) {
      return normalize(format, value.split(','))
    }
    return moment(value, format)
  }
  if (typeof value === 'number') {
    console.warn(value, moment(value, format))
    return moment(value)
  }
  return null
}
function normalizeString(format: string, value: any): string {
  if (Array.isArray(value)) {
    return value.map((v) => normalizeString(format, v)).join(',')
  }
  if (value instanceof moment) {
    return (value as any).utcOffset(480).format(format)
  }
  return value
}

function withMomentHoc<P>(format: string, Component: ComponentClass<P>) {
  const MomentHoc: React.ComponentType<P> = (props: any) => {
    // console.warn((props as any).id, (props as any).value, (props as any).onChange)

    const value = normalize(format, props.value as any)
    const onChange = (value: any, values: any) => {
      let str = normalizeString(format, value)
      ;(props.onChange as any)?.(str, values)
    }
    return <Component {...props} value={value} onChange={onChange} />
  }
  return MomentHoc
}

const {
  MonthPicker: AntdMonthPicker,
  RangePicker: AntdRangePicker,
  WeekPicker: AntdWeekPicker,
} = AntdDatePicker
export const FormRangePicker = withFormItem(withMomentHoc(dateFormat, AntdRangePicker))
export const FormMonthPicker = withFormItem(withMomentHoc(monthFormat, AntdMonthPicker))
export const FormWeekPicker = withFormItem(withMomentHoc(dateFormat, AntdWeekPicker))
export const FormDatePicker = withFormItem(withMomentHoc(dateFormat, AntdDatePicker))
FormRangePicker.defaulItemProps = {placeholder: '请选择'}
FormDatePicker.defaulItemProps = {placeholder: '请选择'}
