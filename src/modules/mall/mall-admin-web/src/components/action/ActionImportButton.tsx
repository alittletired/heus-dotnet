import Upload, { UploadProps } from 'antd'
import ActionButton, { ActionButtonProps } from './ActionButton'
import { importExcel } from '@/utils/excelUtils'
import { useTable } from '../table/Table'
export interface ImportExeclProps<T> extends ActionButtonProps<T> {
  importApi: any
}
function ImportButton<T>(props: ImportExeclProps<T>) {
  let { icon, importApi, ...rest } = props
  let table = useTable()
  if (!icon) {
    icon = 'upload'
  }
  let uploadProps: UploadProps = {
    // action: '/api/public/upload',
    name: 'file',
    beforeUpload: (file) => {
      console.warn('file', file, file.arrayBuffer())

      importExcel({ file, columns: table.columns, importApi })
      return false
    },
    showUploadList: false,
    onChange: async (info) => {
      // console.warn('file', file)
      // let response = info.file.response
      // if (response && response.code === 200) {
      //   let filePath = response.data
      //   await callApi(props, {...apiData, filePath})
      // }
    },
  }

  return (
    <Upload {...uploadProps}>
      <ActionButton onSuccess={() => {}} icon={icon} {...rest} />
    </Upload>
  )
}
export default ImportButton
