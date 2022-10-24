import React from 'react'
import callApi from '@/utils/callApi'
import ActionButton, { ActionButtonProps } from './ActionButton'
import { useTable } from '../table/Table'
import { exportExcel } from '@/utils/excelUtils'
export interface ExportProps {
  fileName?: string
}
export default function ExportButton<D>(props: ActionButtonProps<D> & ExportProps) {
  let { fileName, icon, ...restProps } = props
  let table = useTable()
  if (!icon) {
    icon = 'download'
  }
  return (
    <ActionButton
      icon={icon}
      onSuccess={() => {}}
      {...restProps}
      onClick={async () => {
        let data = await table.props.api({ ...table.props.data, pageSize: -1 })
        await exportExcel({
          sheets: [{ sheetName: table.props.tableTitle, columns: table.columns, data: data.items }],
        })
        // table.search
        // let filePath = await callApi(restProps)
        // window.location.href = `/api/public/download?filePath=${filePath}&fileName=${fileName}.xlsx`
      }}
    />
  )
}
