import React from 'react'
import { getOptionTitle } from '../select'
import { useTable } from './Table'
import { DataIndex } from 'rc-table/lib/interface'
interface OptionCellProps {
  value: any
  dataIndex: DataIndex | symbol
}
const OptionCell: React.FC<OptionCellProps> = (props) => {
  let table = useTable()

  return <>{getOptionTitle(props.value, table.options[props.dataIndex?.toString()])}</>
}
export default OptionCell
