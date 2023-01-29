import React, { ComponentClass } from 'react'
import  dayjs from 'dayjs'
import 'dayjs/locale/zh-cn'
import withFormItem from './withFormItem'
import { TimePicker as AntdTimePicker, DatePicker as AntdDatePicker } from 'antd'
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
    return dayjs(value, format)
  }
  if (typeof value === 'number') {
    console.warn(value, dayjs(value, format))
    return dayjs(value)
  }
  return null
}
function normalizeString(format: string, value: any): string {
  if (Array.isArray(value)) {
    return value.map((v) => normalizeString(format, v)).join(',')
  }
  if (value instanceof dayjs) {
    return (value as any).utcOffset(480).format(format)
  }
  return value
}

function withDayjs<P>(format: string, Component: ComponentClass<P>) {
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
export const FormRangePicker = withFormItem(withDayjs(dateFormat, AntdRangePicker))
export const FormMonthPicker = withFormItem(withDayjs(monthFormat, AntdMonthPicker))
export const FormWeekPicker = withFormItem(withDayjs(dateFormat, AntdWeekPicker))
export const FormDatePicker = withFormItem(withDayjs(dateFormat, AntdDatePicker))
// FormRangePicker.defaultProps = {placeholder: '请选择'}
FormDatePicker.defaultProps = { placeholder: '请选择' }
