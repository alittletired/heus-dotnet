import React from 'react'
import EditableContext from './EditableContext'
import {getId} from '@/utils/dataUtils'
import {Action} from '..'
const OperationCell: React.FC<{data: any}> = ({data}) => {
  const editable = React.useContext(EditableContext)
  if (getId(data) === editable.editingId) {
    return (
      <>
        <Action.Anchor key="save" data={data} onClick={editable.onSave} title="保存" />
        <Action.Anchor
          key="cancel"
          data={data}
          onClick={editable.onCancel}
          title="取消"
        />
      </>
    )
  }
  return (
    <>
      <Action.Anchor data={data} key="edit" onClick={editable.onEdit} title="编辑" />
      {editable.onDelete && (
        <Action.Anchor
          key="delete"
          data={data}
          onClick={editable.onDelete}
          title="删除"
        />
      )}
    </>
  )
}

export default OperationCell
