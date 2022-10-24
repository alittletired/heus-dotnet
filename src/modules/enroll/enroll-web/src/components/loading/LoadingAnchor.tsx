import React, { useCallback, useState } from 'react'
import { ButtonProps } from 'antd'
import LoadingButton from './LoadingButton'
export default function LoadingAnchor(props: ButtonProps) {
  return <LoadingButton type="link" {...props} />
}
