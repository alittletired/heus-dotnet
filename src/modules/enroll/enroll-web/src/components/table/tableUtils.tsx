import React, { ReactNode } from 'react'
import { formatDate, formatDateTime } from '@/utils/dateUtils'
import { getProperty } from '../../utils/dataUtils'
import { ColumnProps } from './interface'
import ActionColumn from './ActionColumn'
import { getOptionTitle, OptionType } from '../select'
import AudioIcon from '../AudioIcon'
import { Image } from '@/components'
const actionWidths = [0, 70, 140, 190, 210]
export function toSearchData(originData: any, columns: ColumnProps[] = []) {
  let data = { ...originData }
  columns.forEach((col) => {
    let key = col.dataIndex as string
    if (col.operator && col.operator !== 'eq' && typeof data[key] !== 'undefined') {
      data[key] = { [`$${col.operator}`]: data[key] }
    }
  })

  return data
}

function fileeDefaultStyle<T>(column: ColumnProps<T>) {
  column.ellipsis = true

  switch (column.valueType) {
    case 'date':
      column.align = column.align ?? 'center'
      column.width = column.width ?? 110
      break
    case 'datetime':
      column.align = column.align ?? 'center'
      column.width = column.width ?? 150
      break
    case 'money':
    case 'number':
      column.align = 'right'
      column.width = column.width ?? 100
      break

    case 'index':
      column = Object.assign(column, {
        width: 70,
        title: '序号',
        dataIndex: '_index' as any,
        align: 'center',
        className: 'table-index-column',
      })
      break
  }

  if (column.actions) {
    column.className = 'column-action'
    column.width = column.width ?? actionWidths[column.actions.length] ?? actionWidths[3]
    column.title = column.title ?? '操作'
  }
  column.className = column.className ?? 'column'
  column.key = `col_${column.title ?? column.valueType}`
}
export function translateColumns<T>(columns: ColumnProps<T>[] = []): ColumnProps<T>[] {
  return columns.map((column) => {
    fileeDefaultStyle(column)

    if (column.render) {
      return column
    }
    if (column.valueType === 'audio') {
      column.render = (value) => <AudioIcon src={value} />
    } else if (column.valueType === 'image') {
      column.render = (value) => <Image src={value} alt="" />
    } else if (column.actions) {
      column.render = (_, data) => <ActionColumn actions={column.actions} data={data} />
    } else if (column.options) {
      let options: OptionType[] = []
      if (typeof column.options == 'function') {
        options = []
      } else {
        options = column.options
      }
      column.render = (value) => getOptionTitle(value, options)
    } else if (column.valueType && column.dataIndex) {
      column.render = (value, data) => {
        if (column.valueType === 'date') {
          return formatDate(value)
        } else if (column.valueType === 'datetime') {
          return formatDateTime(value)
        } else if (column.valueType === 'arrayJoin') {
          if (!value) return ''
          return value.join(',')
        }
        return value
      }
    } else if (column.dataPath) {
      column.render = (text, data) => {
        let paths = column.dataPath!.split('.')
        let value = getProperty(data, paths[0])
        if (paths.length > 1) value = value[paths[1]]
        if (paths.length > 2) value = value[paths[2]]
        return value
      }
    }
    return column
  })
}
