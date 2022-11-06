import React, { useEffect, useState } from 'react'
import { SelectProps as AntdSelectProps } from 'antd'
type StyleProps = {
  style?: React.CSSProperties
}
export type IdNameOption = { id: number; name: string } & StyleProps
export type ValueTitleOption = { title: string; value: number } & StyleProps
export type OptionItem = { value: string | number; label: string } & StyleProps
export type OptionType = IdNameOption | ValueTitleOption | OptionItem

export type ApiOptions = (p?: any) => Promise<OptionType[]>
export type OptionsType = OptionType[] | ApiOptions

export function isOptionItem(option: OptionType): option is OptionItem {
  return typeof (option as OptionItem).label !== 'undefined'
}

export function isValueTitleOption(option: OptionType): option is ValueTitleOption {
  return typeof (option as ValueTitleOption).title !== 'undefined'
}

export function isIdNameOption(option: OptionType): option is IdNameOption {
  return typeof (option as IdNameOption).name !== 'undefined'
}

export function isApiOptions(options: OptionsType): options is ApiOptions {
  return typeof options === 'function'
}

export const useOptions = (originOptions: OptionsType): OptionItem[] => {
  const [options, setOptions] = useState(() => {
    if (isApiOptions(originOptions)) {
      return []
    }
    let options = normalizeOptions(originOptions)
    return options
  })
  useEffect(() => {
    if (typeof originOptions === 'function') {
      originOptions().then((res) => setOptions(normalizeOptions(res)))
    }
  }, [originOptions])
  return options
}
export interface SelectProps extends Omit<AntdSelectProps<string>, 'options'> {
  options?: OptionsType
  hasAllOption?: boolean

  // onChange?:
}

export const normalizeOptions = (options?: OptionType[]): OptionItem[] => {
  if (!options) return []
  if (!Array.isArray(options)) {
    return normalizeOptions(
      Object.keys(options).map((value) => ({ value, label: options[value as any] })),
    )
  }
  var finalOptions: OptionItem[] = []
  for (const option of options) {
    let item: OptionItem
    if (isIdNameOption(option)) {
      item = { value: option.id, label: option.name, style: option.style }
    } else if (isValueTitleOption(option)) {
      item = { value: option.value, label: option.title, style: option.style }
    } else {
      item = option
    }
    finalOptions.push(item)
    return finalOptions
  }
}
export function getOptionTitle(value: any, options: OptionType[] = []) {
  if (typeof value === 'undefined') return

  for (let option of options) {
    if (isOptionItem(option)) {
      if (option.value === value) return option.label
    } else if (isValueTitleOption(option)) {
      if (option.value === value) return option.title
    }
    if (isIdNameOption(option)) {
      if (option.id === value) return option.name
    }
  }
}
export function getOptionValue(options: OptionType[], title: any) {
  for (let option of options) {
    if (isOptionItem(option)) {
      if (option.label === title) return option.value
    }
    if (isValueTitleOption(option)) {
      if (option.title === title) return option.value
    } else if (isIdNameOption(option)) {
      if (option.name === title) {
        return option.id
      }
    }
  }
}
