import React, {useContext} from 'react'
import EditableContext, {EditColumn} from './EditableContext'
import {editControls} from './editData'
import {getId} from '@/utils/dataUtils'
interface EditableCellProps extends React.HTMLAttributes<HTMLElement>, EditColumn {
  title: any
  index: number
  record: any
  editing: boolean
}
const EditableCell: React.FC<EditableCellProps> = ({
  title,
  children,
  dataIndex,
  editType,
  editProps,
  record,
  ...rest
}) => {
  const editable = React.useContext(EditableContext)
  let childNode = children

  if (editType && getId(record) === editable.editingId) {
    let Component = editControls[editType]
    childNode = (
      <Component {...editProps} name={dataIndex} style={{margin: 0}} label={title} />
    )
  }
  // let col = translate(props)
  return <td {...rest}>{childNode}</td>
  // return <td {...restProps}></td>
}
export default EditableCell
