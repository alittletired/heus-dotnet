import React, { ReactNode } from 'react'
import { formatDate, formatDateTime } from '@/utils/dateUtils'
import { getProperty } from '../../utils/dataUtils'
import { ColumnProps } from './interface'
import ActionColumn from './ActionColumn'
import { getOptionTitle, OptionType } from '../select'
import AudioIcon from '../AudioIcon'
import { Image } from '@/components'
const actionWidths = [0, 70, 140, 190, 210]
function isDynamicSearchFilter(value: any): value is DynamicSearchFilter<any, any> {
  return typeof value === 'object' && 'op' in value
}
export function toSearchData<T>(
  originData: T,
  columns: ColumnProps[],
  page: PageRequest,
): DynamicSearch<T> {
  const searchData: DynamicSearch<T> = { filters: {} }
  const ops = new Map(
    columns
      .filter((col) => col.dataIndex && col.operator)
      .map((col) => [col.dataIndex!, col.operator!]),
  )
  for (let key in originData) {
    let value = originData[key]
    if (!value) {
      continue
    }

    let op: any = ops.get(key) ?? 'eq'
    //允许构造参数时，自行使用高级查询
    if (isDynamicSearchFilter(value)) {
      searchData.filters[key] = { ...value }
    } else {
      searchData.filters[key] = { op, value }
    }
  }

  return { ...searchData, ...page }
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
  column.key = column.key ?? `col_${column.dataIndex}`
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
