import React from 'react'
import { ToolBarItem } from './interface'
import { ActionComponent } from '../action'
import { useTable } from './Table'
interface Props {
  items?: ToolBarItem[]
  data?: any
}
const ToolBar: React.FC<Props> = (props) => {
  const table = useTable()
  let { items = [] } = props

  const itemsDom = items.map((item, idx) => {
    let { disabled, type, ...rest } = item
    if (item.buttonType === 'create') {
      type = type ?? 'primary'
    }
    return (
      <ActionComponent
        data={table.data}
        key={item.code || idx}
        type={type}
        onSuccess={() => table.reload()}
        {...rest}
      />
    )
  })

  return (
    <div className="toobar">
      <div className="toolbar-option">{itemsDom}</div>
    </div>
  )
}
export default ToolBar
