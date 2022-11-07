import React, { PropsWithChildren, useEffect, useMemo } from 'react'
import { Layout } from 'antd'
import GlobalHeader from './GlobalHeader'
import SiderMenu from './SiderMenu'
import useRouter from '@/services/router'
const { Content } = Layout

const ProtectedLayout: React.FC<PropsWithChildren> = (props) => {
  const router = useRouter()
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
