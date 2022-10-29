import React, { PropsWithChildren, useEffect, useMemo } from 'react'
import { Layout } from 'antd'
import GlobalHeader from './GlobalHeader'
import SiderMenu from './SiderMenu'
import { useUser } from '@/services/user'
import { useAppConfig } from './appConfig'
import useRouter from '@/services/router'
const { Content } = Layout

const ProtectedLayout: React.FC<PropsWithChildren> = (props) => {
  const [user] = useUser()
  const [appConfig] = useAppConfig()
  const router = useRouter()

  if (!user.isLogin && router.asPath !== appConfig.loginUrl) {
    // console.warn(router, router.pathname, appConfig.loginUrl)

    router.replace(appConfig.loginUrl)
    return <div></div>
  }
  if (router.query['hideMenu']) return null
  return (
    <Layout hasSider style={{ minHeight: '100%' }} className="global">
      <div className="global-left"></div>
      <SiderMenu />
      <Layout style={{ position: 'relative' }}>
        <GlobalHeader />
        <Content className="global-content">{props.children}</Content>
      </Layout>
    </Layout>
  )
}
export default ProtectedLayout
