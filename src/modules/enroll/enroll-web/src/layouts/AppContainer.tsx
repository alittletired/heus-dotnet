import React, { useEffect } from 'react'
import { ConfigProvider } from 'antd'
import zhCN from 'antd/es/locale/zh_CN'
import { getMenuByPath } from '@/services/menu'
import { RecoilRoot } from 'recoil'
import withLayout from './withLayout'
import { useAppConfig } from './appConfig'

interface Props<P> {
  pageProps: P
  Component: React.ComponentType<P>
}
function AppContainer<P>(props: Props<P>) {
  const [appConfig] = useAppConfig()
  useEffect(() => {
    const pathname = location.pathname
    let currMenu = getMenuByPath(pathname)
    document.title = currMenu?.name || appConfig.siteName
  }, [appConfig.siteName])
  const LayoutComponent = withLayout(props.Component)
  return (
    <RecoilRoot>
      <ConfigProvider locale={zhCN}>
        <LayoutComponent {...props.pageProps} />
      </ConfigProvider>
    </RecoilRoot>
  )
}
export default AppContainer
