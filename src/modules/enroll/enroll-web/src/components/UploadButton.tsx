import {getAuth} from '@/services/auth'
import {Button, Upload} from 'antd'
import {UploadProps} from 'antd/lib/upload'
import React, {useState} from 'react'
import {UploadOutlined} from './icons'
interface Props {
  title: string
  onSuccess?: (fileUrl: string) => any
}
const UploadButton: React.FC<Props> = (props) => {
  const [loading, setLoading] = useState(false)
  const onChange: UploadProps['onChange'] = ({file}) => {
    if (file.status === 'done') {
      file.response.code === 200 && props.onSuccess?.(file.response.data)
      setLoading(false)
    } else {
      setLoading(true)
    }
  }
  const auth = getAuth()
  return (
    <Upload
      disabled={loading}
      name="file"
      action="/api/file/upload"
      headers={{
        Authorization: 'Bearer ' + auth.accessToken,
        userId: auth.userId.toString(),
      }}
      showUploadList={false}
      onChange={onChange}>
      <Button loading={loading} icon={<UploadOutlined />}>
        {props.title}
      </Button>
    </Upload>
  )
}
export default UploadButton
