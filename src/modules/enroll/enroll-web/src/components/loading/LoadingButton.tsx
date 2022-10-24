import React, {useCallback, useState} from 'react'
import {Button} from 'antd'
import {ButtonProps} from 'antd/lib/button'
export default function LoadingButton(props: ButtonProps) {
  const [loading, setLoading] = useState(false)
  const {onClick} = props
  const handeClick = useCallback(
    async (e: any) => {
      setLoading(true)
      try {
        await onClick(e)
      } finally {
        setLoading(false)
      }
    },
    [onClick],
  )
  return <Button {...props} loading={loading} onClick={handeClick} />
}
