import React, {useCallback, useEffect, useRef, useState} from 'react'
import {Button, Input, message} from 'antd'
import withFormItem from './withFormItem'
import adminApi, {VerifyCodeType} from '@/api/admin'
import {InputProps} from 'antd/lib/input'

interface VerifyCodeProps extends InputProps {
  codeType: VerifyCodeType
  mobile: string
}

const COUNT_DOWN = 61

const FormVerifyCode = withFormItem((props: VerifyCodeProps) => {
  const {codeType, mobile, ...inputProps} = props
  const [countdown, setCountdown] = useState(COUNT_DOWN)
  const [loading, setLoading] = useState(false)
  const listenerRef = useRef(null)

  const clearListener = useRef(() => {
    listenerRef.current && clearInterval(listenerRef.current)
  })

  const startListener = useRef(() => {
    listenerRef.current = setInterval(() => {
      setCountdown((prev) => {
        if (prev === 1) {
          clearListener.current()
          return COUNT_DOWN
        }
        return prev - 1
      })
    }, 1000)
  })

  const sendVerifyCode = useCallback(async () => {
    setLoading(true)
    try {
      await adminApi.accounts.sendVerifyCode({
        type: props.codeType,
        phone: props.mobile,
      })
      setCountdown(COUNT_DOWN - 1)
      setLoading(false)
      startListener.current()
    } catch (err: any) {
      message.warning(err.data.msg)
      setLoading(false)
    }
  }, [props])

  useEffect(() => {
    var curr = clearListener.current
    return () => {
      curr()
    }
  }, [])

  return (
    <React.Fragment>
      <Input {...inputProps} name="verifyCode" style={{width: 120, marginRight: 20}} />
      <Button
        loading={loading && countdown >= COUNT_DOWN}
        disabled={countdown < COUNT_DOWN}
        type="primary"
        onClick={sendVerifyCode}>
        {loading
          ? '发送中...'
          : countdown < COUNT_DOWN
          ? `${countdown}秒后发送`
          : '发送验证码'}
      </Button>
    </React.Fragment>
  )
})

export default FormVerifyCode
