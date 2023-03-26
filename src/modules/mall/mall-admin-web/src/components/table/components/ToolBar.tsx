import React from 'react'
import { ToolBarItem } from '../types'
import { ActionComponent } from '../../action'
import { useTable } from '../Table'
import { Space } from 'antd'
interface Props<T> {
  items?: ToolBarItem[]
  data?: T
}
function ToolBar<T>(props: Props<T>) {
  const table = useTable()
  let { items = [] } = props

  const itemsDom = items.map((item, idx) => {
    let { disabled, type, ...rest } = item
    if (item.actionName === 'create') {
      type = type ?? 'primary'
    }
    return (
      <ActionComponent
        data={table.data}
        key={item.actionName || idx}
        type={type}
        onSuccess={() => table.search()}
        {...rest}
      />
    )
  })

  return (
    <div className="toobar">
      <Space>{itemsDom}</Space>
    </div>
  )
}
export default ToolBar
