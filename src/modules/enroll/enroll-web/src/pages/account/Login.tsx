import React, { useState } from 'react'
import { Alert, Spin, Tabs } from 'antd'
import { LoginInput } from '@/api/admin'
import { useUser, login } from '@/services/user'
import { Form, FormItem, Link } from '@/components'
import { LockTwoTone, MobileOutlined, UserOutlined } from '@ant-design/icons'
import useRouter from '@/services/router'
import UserLayout from '@/layouts/UserLayout'
import styles from './login.module.css'
/*防止自动填充 */
const PasswordInput: React.FC = () => {
  return (
    <React.Fragment>
      <input style={{ opacity: 0, position: 'absolute' }} />
      <input type="password" style={{ opacity: 0, position: 'absolute' }} />
    </React.Fragment>
  )
}

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
const Login: PageComponent = () => {
  const [user] = useUser()
  const router = useRouter()
  const [loginType, setLoginType] = useState<LoginType>('account')
  let model: LoginInput = {
    userName: '',
    password: '',
    rememberMe: false,
  }
  if (user.isLogin) {
    router.replace((router.query['redirect'] as string) ?? '/')
    return <Spin />
  }

  return (
    <UserLayout containerClass={styles.main}>
      <Form initialValues={model} noLabel api={login}>
        <Tabs
          destroyInactiveTabPane
          animated={false}
          className={styles.tabs}
          activeKey={loginType}
          onChange={(value) => setLoginType(value as LoginType)}>
          <Tabs.TabPane key="account" tab="账户密码登录">
            <PasswordInput />
            <FormItem.Input
              name="userName"
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
          </Tabs.TabPane>
        </Tabs>

        <FormItem.Item>
          <FormItem.Checkbox name="rememberMe">自动登录</FormItem.Checkbox>
          <Link className={styles.forgetPassword} href="/user/forget-password">
            忘记密码
          </Link>
        </FormItem.Item>
        <FormItem.SubmitButton size="large" className={styles.submit} title="登录" />

        <div className={styles.other}>
          <Link className={styles.register} href="/user/register">
            注册账户
          </Link>
        </div>
      </Form>
    </UserLayout>
  )
}
Login.options = {
  layout: 'empty',
}
export default Login
