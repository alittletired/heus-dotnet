import React, { PropsWithChildren, useEffect, useMemo } from 'react'
import { Layout } from 'antd'
import GlobalHeader from './GlobalHeader'
import SiderMenu from './SiderMenu'
import { useUser } from '@/services/user'
import { useAppConfig } from './appConfig'
import { ItemType } from 'antd/es/menu/hooks/useItems'
import useRouter from '@/services/router'
const { Content } = Layout

const ProtectedLayout: React.FC<PropsWithChildren> = (props) => {
  const [user] = useUser()
  const [appConfig] = useAppConfig()
  const router = useRouter()
  const menus: ItemType[] = useMemo(() => [], [])
  useEffect(() => {
    if (!user.isLogin) router.replace(appConfig.loginUrl)
  }, [router, user.isLogin, appConfig.loginUrl])
  const menu = useMemo(() => {
    if (router.query['hideMenu']) return null
    return (
      <>
        <div className="globalLeft"></div>
        <SiderMenu menus={menus} />
      </>
    )
  }, [menus, router.query])

  return (
    <Layout hasSider style={{ minHeight: '100%' }}>
      {menu}

      <Layout style={{ position: 'relative' }}>
        <GlobalHeader />
        <Content className="globalcontent">{props.children}</Content>
      </Layout>
    </Layout>
  )
}
export default ProtectedLayout
