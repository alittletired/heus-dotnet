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

export type ToolBarItem<T = any> = ActionComponentProps<T> & {
  reloadData?: boolean
}
export interface TableProps<T, P> extends Omit<AntdTableProps<T>, 'columns'> {
  useTreeTable?: boolean
  titles?: { [key in string]: string }
  tableTitle: string
  columns?: ColumnProps<T>[]
  tableHeader?: React.ReactNode
  toolBar?: ToolBarItem[]
  api?: (param: P) => Promise<PageList<T>>
  params?: Partial<P>
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
