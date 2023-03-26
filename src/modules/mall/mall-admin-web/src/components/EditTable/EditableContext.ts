import React from 'react'
import { ColumnProps } from '../table/types'
import { EditType } from './editData'
import { FormItemProps } from '../form/withFormItem'
export interface EditableContext<T> {
  editingId: number
  onSave?: (data: T) => Promise<any>
  onCancel?: (data: T) => Promise<any>
  onEdit?: (data: T) => Promise<any>
  onDelete?: (data: T) => Promise<any>
}
const EditableContext = React.createContext({} as EditableContext<any>)
export interface EditColumn<T = any> extends ColumnProps<T> {
  editType?: EditType
  editProps?: FormItemProps
}
export default EditableContext
