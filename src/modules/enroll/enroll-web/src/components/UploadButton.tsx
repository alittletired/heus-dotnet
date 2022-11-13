import { getAuthHeaders } from '@/services/http'
import { getUser, useUser } from '@/services/user'
import { Button, Upload, UploadProps } from 'antd'
import React, { useState } from 'react'
import { UploadOutlined } from './icons'
interface Props {
  title: string
  onSuccess?: (fileUrl: string) => any
}
const UploadButton: React.FC<Props> = (props) => {
  const [loading, setLoading] = useState(false)
  const onChange: UploadProps['onChange'] = ({ file }) => {
    if (file.status === 'done') {
      file.response.code === 200 && props.onSuccess?.(file.response.data)
      setLoading(false)
    } else {
      setLoading(true)
    }
  }
  return (
    <Upload
      disabled={loading}
      name="file"
      action="/api/file/upload"
      headers={getAuthHeaders()}
      showUploadList={false}
      onChange={onChange}>
      <Button loading={loading} icon={<UploadOutlined />}>
        {props.title}
      </Button>
    </Upload>
  )
}
export default UploadButton
