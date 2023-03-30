import React, { useState, useCallback } from 'react'
import withFormItem from './withFormItem'
import icons from '../icons'
import { Upload, message, UploadProps, UploadFile } from 'antd'

import { UploadChangeParam } from 'antd/lib/upload/interface'
import { useGlobalConfig } from '@/services/globalConfig'
interface AvatarProps extends Omit<UploadProps, 'onChange'> {
  value?: string
  onChange?: (value: string) => void
}
const FormAvatar = withFormItem((props: AvatarProps, ref: any) => {
  let { onChange, value, ...rest } = props
  const [loading, setLoading] = useState(false)
  const [globalConfig] = useGlobalConfig()
  const uploadButton = (
    // @ts-expect-error
    <div>
      {loading ? icons.loading : icons.plus}
      <div className="ant-upload-text">点击上传</div>
    </div>
  )

  const beforeUpload = useCallback((file: UploadFile) => {
    const isJpgOrPng = file.type === 'image/jpeg' || file.type === 'image/png'
    if (!isJpgOrPng) {
      message.error('只允许上传图片文件!')
    }

    const isLt2M = file.size! / 1024 / 1024 < 2
    if (!isLt2M) {
      message.error('图片大小必须小于2MB!')
    }
    return isJpgOrPng && isLt2M
  }, [])
  const handleChange = useCallback(
    (info: UploadChangeParam) => {
      if (info.file.status === 'uploading') {
        setLoading(true)
        return
      }
      if (info.file.status === 'done') {
        setLoading(false)
        if (info.file.response?.data) onChange?.(info.file.response?.data)
        // Get this url from response in real world.
        console.warn('file upload', info)
      }
    },
    [onChange],
  )
  return (
    <Upload
      style={{ width: '128px', height: '128px' }}
      name="avatar"
      listType="picture-card"
      className="avatar-uploader"
      showUploadList={false}
      action={`${globalConfig.apiBaseUrl}/api/pb/file/upload-image`}
      beforeUpload={beforeUpload}
      {...rest}
      onChange={handleChange}>
      {props.value ? (
        <img
          src={`${globalConfig.apiBaseUrl}${props.value}`}
          alt="avatar"
          style={{ width: '100%' }}
        />
      ) : (
        uploadButton
      )}
    </Upload>
  )
})
export default FormAvatar
