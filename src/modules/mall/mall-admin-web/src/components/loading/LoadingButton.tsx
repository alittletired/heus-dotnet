import React, { useCallback, useState } from 'react'
import { Button, ButtonProps } from 'antd'
export default function LoadingButton(props: ButtonProps) {
  const [loading, setLoading] = useState(false)
  const { onClick } = props
  const handeClick = useCallback(
    async (e: any) => {
      setLoading(true)
      try {
        await onClick?.(e)
      } finally {
        setLoading(false)
      }
    },
    [onClick],
  )
  return <Button {...props} loading={loading} onClick={handeClick} />
}
