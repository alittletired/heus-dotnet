import React, { ReactNode, Dispatch, SetStateAction } from 'react'
import { TableProps as AntdTableProps } from 'antd'
import { ActionComponentProps } from '../action'
import { OptionsType, NameOption } from '../select'
import { TreeNodeData } from '../tree/treeUtils'
import { ColumnType } from 'antd/es/table'
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
export type TableApi<T, P> = (param?: P) => Promise<PageList<T>>
export type TableApiSearch<P> = (param?: Partial<P>) => void

export type OnApiBefore<P> = (param: P) => Promise<boolean> | boolean
export type OnApiSuccess<T> = (data: PageList<T>) => any
export interface TableInstance<T = any, P = any> {
  props: TableProps<T, P>
  reload: TableApiSearch<P>
  reset: TableApiSearch<P>
  search: TableApiSearch<P>
  columns: ColumnProps<T>[]
  dataSource: T[] | TreeNodeData<T>[]
  disableAutoReload?: boolean
  options: Record<string, NameOption[]>
  setDataSource: Dispatch<SetStateAction<T[]>>
  //当前的请求参数
  data: P
}

export type ToolBarItem<T = any> = ActionComponentProps<T> & {
  disableAutoReload?: boolean
}
export interface TableProps<T, P extends PageRequest = {}>
  extends Omit<AntdTableProps<T>, 'columns'> {
  useTreeTable?: boolean
  titles?: { [key in string]: string }
  table?: TableInstance<T>
  tableTitle: string
  columns?: ColumnProps<T>[]
  tableHeader?: React.ReactNode
  toolBar?: ToolBarItem[]
  onApiBefore?: OnApiBefore<P>
  api?: (param?: P) => Promise<PageList<T>>
  data?: P
  OnApiSuccess?: OnApiSuccess<T>
  hiddenIndexColumn?: boolean
}

export interface ColumnProps<T = any> extends Omit<ColumnType<T>, 'dataIndex'> {
  valueType?: ValueType
  options?: OptionsType
  optionsParam?: any
  actions?: ToolBarItem<T>[]
  dataPath?: string
  operator?: Operator
  sortable?: boolean
  dataIndex?: keyof T | string
}
