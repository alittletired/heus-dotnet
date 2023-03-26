import React, { useCallback, useState, useMemo, useEffect, useRef } from 'react'
import { idIsEqual } from '@/utils/dataUtils'

import EditableContext, { EditColumn } from './EditableContext'
import EditableCell from './EditableCell'
import ApiForm from '../form/Form'
import { Action, overlay } from '..'
import { getId, setId } from '@/utils/dataUtils'
import { TableProps } from '../table/types'
import ApiTable from '../table'
import { useTable } from '../table/Table'
import OperationCell from './OperationCell'

interface EditTableProps<T> extends Omit<TableProps<T>, 'columns'> {
  columns: EditColumn<T>[]
  deleteApi?: (data: T) => Promise<any>
  updateApi?: (data: T) => Promise<any>
  createApi?: (data: T) => Promise<any>
}
const components = {
  body: {
    cell: EditableCell,
  },
}
// const EditableContext = React.createContext<any>();
export default function EditTable<T extends object>(props: EditTableProps<T>) {
  const [loading, setLoading] = useState(false)
  const { api, deleteApi, updateApi, createApi, ...rest } = props
  const defaultData = useMemo(() => {
    let defaultData: Record<string, any> = {}
    for (let key of Object.keys(props.data)) {
      if (typeof props.data[key] !== 'object') {
        defaultData[key] = props.data[key]
      }
    }
    return defaultData
  }, [props.data])
  const [form] = ApiForm.useForm()
  const table = useTable()
  const [editingId, setEditingId] = useState<number | null>(null)

  const onDelete = useRef(async (data: T) => {
    if (await overlay.deleteConfirm()) {
      if (getId(data) > 0) {
        await deleteApi?.(data)
      }
      table.setDataSource((prv) => prv.filter((item) => !idIsEqual(item, data)))
    }
  })
  const onSave = useRef(async (record: T) => {
    let store = await form.validateFields()
    let data = { ...record, ...store }
    let saveApi = getId(record) > 0 ? updateApi : createApi
    setId(data, getId(record) > 0 ? getId(record) : undefined)
    data = await saveApi?.(data)
    setEditingId(null)
    table.setDataSource((ds) => {
      if (getId(record) < 0) {
        ds[ds.length - 1] = data
      } else {
        const index = ds.findIndex((item) => getId(item) === getId(data))
        if (index > -1) {
          ds[index] = data
        } else {
          console.error('不应该执行到这里', ds, data, record)
        }
      }

      return [...ds]
    })
  })
  const onCancel = useRef(async () => {
    setEditingId(null)
  })
  const onEdit = useRef(async (data: T) => {
    setEditingId(getId(data))
    form.setFieldsValue(data)
  })
  const columns = useMemo(() => {
    // let options = [{title: '编辑',onClick:onEdit.current},{title: '删除',onClick:onDelete.current}]
    let columns = [
      ...props.columns,
      {
        title: '操作',
        width: '150px',
        render: (_: any, record: T) => <OperationCell data={record} />,
      } as unknown as EditColumn,
    ]
    return columns.map((col) => {
      return {
        ...col,
        onCell: (record: T) => ({
          record,
          editType: col.editType,
          editProps: col.editProps,
          dataIndex: col.dataIndex,
          title: col.title as any,
        }),
      }
    })
  }, [props.columns])
  const onAdd = useRef(async () => {
    table.setDataSource((prev) => {
      if (prev.length > 0 && getId(prev[prev.length - 1]) < 0) {
        return prev
      }
      let id = -prev.length - 1
      let data = { ...defaultData, id }
      setEditingId(id)
      let fields = rest.columns.filter((c) => c.dataIndex).map((c) => c.dataIndex)
      //@ts-ignore
      form.resetFields(fields)
      form.setFieldsValue(data)
      return [...prev, data]
    })
  })
  const toolBar = useMemo(() => {
    let toolBar = props.toolBar || []
    if (props.createApi) {
      toolBar.unshift({ title: '新增', onClick: onAdd.current, actionType: 'create' })
    }
    return toolBar
  }, [props.toolBar, props.createApi])

  useEffect(() => {
    api(rest.data).then((data) => {
      table.setDataSource(data.items)
    })
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])
  return (
    <EditableContext.Provider
      value={{
        onSave: updateApi && onSave.current,
        onCancel: onCancel.current,
        onDelete: deleteApi && onDelete.current,
        onEdit: onEdit.current,
        editingId,
      }}>
      <ApiForm form={form} component={false} noLabel>
        <ApiTable
          {...rest}
          toolBar={toolBar}
          components={components}
          loading={loading}
          table={table}
          columns={columns}
          rowClassName="editable-row"
        />
        <style jsx global>{`
          .ant-table-tbody .editable-row .ant-table-cell {
            padding: 4px 11px;
          }
          .ant-table-tbody .editable-row .ant-form-item {
            margin: -3px -8px;
          }
          .editable-row .ant-input {
            padding: 3px 8px;
          }
          .editable-row:hover .editable-cell-value-wrap {
            border: 1px solid #d9d9d9;
            border-radius: 4px;
            padding: 4px 11px;
          }

          [data-theme='dark'] .editable-row:hover .editable-cell-value-wrap {
            border: 1px solid #434343;
          }
          .ant-table.ant-table-small .ant-table-tbody > tr.editable-row > td {
            padding: 2px 8px;
          }
        `}</style>
      </ApiForm>
    </EditableContext.Provider>
  )
}
