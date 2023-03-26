import React, {useState, useEffect, useCallback} from 'react'
import {Row, Col, Button, Form, Input, message} from 'antd'
import FormContext from '@/components/form/FormContext'
import adminApi from '@/api/admin'
import {isPhone} from '@/utils/validate'
// import trainApi, {VerifyCodeType} from '@/api/train'
interface Props {
  countDown?: number
}
const Captcha: React.FC<Props> = ({countDown = 60}) => {
  const [count, setCount] = useState(countDown)
  const [timing, setTiming] = useState(false)
  const formContext = React.useContext(FormContext)
  const onGetCaptcha = useCallback(async (mobile: string) => {
    if (!isPhone(mobile)) {
      return message.error('无效的手机号码')
    }
    // trainApi.students.pb.sendVerifyCode({phone: mobile, type: VerifyCodeType.PRE_SCHOOL})
    setTiming(true)
  }, [])
  useEffect(() => {
    let interval = 0
    if (timing) {
      interval = window.setInterval(() => {
        setCount((preSecond) => {
          if (preSecond <= 1) {
            setTiming(false)
            clearInterval(interval)
            // 重置秒数
            return countDown || 60
          }
          return preSecond - 1
        })
      }, 1000)
    }
    return () => clearInterval(interval)
  }, [timing, countDown])
  return (
    <Form.Item shouldUpdate noStyle>
      <Row gutter={8}>
        <Col span={16}>
          <Form.Item name="captcha" required>
            <Input size="large" />
          </Form.Item>
        </Col>
        <Col span={8}>
          <Button
            size="large"
            disabled={timing}
            style={{display: 'block', width: '100%'}}
            onClick={() => {
              console.warn(formContext.form)

              const value = formContext.form.getFieldValue('phone')
              onGetCaptcha(value)
            }}>
            {timing ? `${count} s` : '获取验证码'}
          </Button>
        </Col>
      </Row>
    </Form.Item>
  )
}
export default Captcha
