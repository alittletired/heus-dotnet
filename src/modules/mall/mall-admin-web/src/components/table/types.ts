import React, { ReactNode, Dispatch, SetStateAction } from 'react'
import { TableProps as AntdTableProps } from 'antd'
import { ActionComponentProps } from '../action'
import { OptionsType, NameOption } from '../select'
import { TreeNodeData } from '../tree/treeUtils'
import { ColumnType } from 'antd/es/table'
import { ActionButtonProps } from '../action/ActionButton'
export type ValueType =
  | 'date'
  | 'money'
  | 'datetime'
  | 'arrayJoin'
  | 'index'
  | 'image'
  | 'number'
  | 'hidden'
  | 'audio'
export type TableApiSearch<P> = (param?: Partial<P>) => void

export type ToolBarItem<T = any> = ActionComponentProps<T> & {
  autoReload?: boolean
}
export interface TableProps<T> extends Omit<AntdTableProps<T>, 'columns'> {
  useTreeTable?: boolean
  titles?: { [key in string]: string }
  tableTitle: string
  columns: ColumnProps<T>[]
  tableHeader?: React.ReactNode
  toolBar?: ToolBarItem[]
  request?: (data: DynamicSearch<T>) => Promise<PageList<T>>
  beforeRequest?: (data: Partial<T>) => false | T | Promise<false | T>
  data?: Partial<T>
  hiddenIndexColumn?: boolean
}

export type ColumnAction<T> = Omit<ActionButtonProps<T>, 'onClick' | 'data'> & {
  autoReload?: boolean
  onClick?: (data: T) => Promise<any>
}
export interface ColumnProps<T = any> extends Omit<ColumnType<T>, 'dataIndex'> {
  valueType?: ValueType
  options?: OptionsType
  optionsParam?: any
  actions?: ColumnAction<T>[]
  dataPath?: string
  operator?: Operator
  sortable?: boolean
  dataIndex?: keyof T
}
