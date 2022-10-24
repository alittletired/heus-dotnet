import React, { useState, useEffect, useLayoutEffect } from 'react'
import styles from './login.module.less'
import adminApi, { LoginVO, AuthTokenDTO } from '@/api/admin'
import { useAuth, login } from '@/services/auth'
import { Alert, Tabs } from 'antd'
import { Form, FormItem } from '@/components'
import { Link, useQuery } from '@/utils/routerUtils'
import UserLayout from '../user/components/UserLayout'
import CaptCha from '../user/components/Captcha'
import { LockTwoTone, MobileOutlined, UserOutlined } from '@ant-design/icons'
import DisableAutoFill from '../user/components/DisableAutoFill'
const LoginMessage: React.FC<{
  content: string
}> = ({ content }) => (
  <Alert
    style={{
      marginBottom: 24,
    }}
    message={content}
    type="error"
    showIcon
  />
)
type LoginType = 'account' | 'mobile'
const Login = () => {
  const [auth] = useAuth()

  const query = useQuery()
  const [loginType, setLoginType] = useState<LoginType>('account')
  let model: LoginVO = {
    account: '',
    password: '',
    rememberMe: false,
  }
  if (auth.isLogin) {
    return <Navigate to={query.get('redirect') ?? '/'} />
  }
  const onSuccess = (data: AuthTokenDTO) => {
    login(data)
  }

  return (
    <UserLayout containerClass={styles.main}>
      <Form onSuccess={onSuccess} initialValues={model} noLabel api={adminApi.accounts.login}>
        <Tabs
          destroyInactiveTabPane
          animated={false}
          className={styles.tabs}
          activeKey={loginType}
          onChange={(value) => setLoginType(value as LoginType)}>
          <Tabs.TabPane key="account" tab="账户密码登录">
            <DisableAutoFill />
            <FormItem.Input
              name="account"
              size="large"
              required
              placeholder="用户名"
              prefix={<UserOutlined className={styles.userIcon} />}
            />
            <FormItem.Input
              name="password"
              type="password"
              required
              size="large"
              placeholder="密码"
              autoComplete="new-password"
              prefix={<LockTwoTone className={styles.prefixIcon} />}></FormItem.Input>
          </Tabs.TabPane>
          <Tabs.TabPane key="mobile" tab="手机号登录">
            <FormItem.Input
              name="phone"
              size="large"
              required
              placeholder="手机号"
              prefix={<MobileOutlined className={styles.prefixIcon} />}
            />
            <CaptCha />
          </Tabs.TabPane>
        </Tabs>

        <FormItem.Item>
          <FormItem.Checkbox name="rememberMe">自动登录</FormItem.Checkbox>
          <Link className={styles.forgetPassword} to="/user/forget-password">
            忘记密码
          </Link>
        </FormItem.Item>
        <FormItem.SubmitButton size="large" className={styles.submit} title="登录" />

        <div className={styles.other}>
          <Link className={styles.register} to="/user/register">
            注册账户
          </Link>
        </div>
        {/* <FormItem.SubmitButton size="large" className={styles.submit} title="登录" /> */}
      </Form>
    </UserLayout>
  )
}
export default Login
