import React, { useEffect, useMemo } from 'react'
import { Layout } from 'antd'
import GlobalHeader from './GlobalHeader'
import SiderMenu from './SiderMenu'
import { useAuth } from '@/services/auth'
import { useAppConfig } from './appConfig'
import { ItemType } from 'antd/es/menu/hooks/useItems'
import useRouter from '@/services/router'
const { Content } = Layout
interface Props {
  menus: ItemType[]
  children: React.ReactNode
}

const ProtectedLayout: React.FC<Props> = (props) => {
  const [auth] = useAuth()
  const appConfig = useAppConfig()
  const router = useRouter()
  useEffect(() => {
    if (!auth.isLogin) router.replace(appConfig.loginUrl)
  }, [router, auth.isLogin, appConfig.loginUrl])
  const menu = useMemo(() => {
    if (router.query['hideMenu']) return null
    return (
      <>
        <div className="globalLeft"></div>
        <SiderMenu menus={props.menus} />
      </>
    )
  }, [props.menus, router.query])

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
