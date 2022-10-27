import React, { useEffect, useState } from 'react'
import { ConfigProvider } from 'antd'
// import zhCN from 'antd/es/locale/zh_CN'
import { getMenuByPath } from '@/services/menu'
import { RecoilRoot } from 'recoil'
import withLayout from './withLayout'
import { useAppConfig } from './appConfig'
import { LoadPermission } from '@/services/permissions'

interface Props<P> {
  pageProps: P
  Component: React.ComponentType<P>
}
function AppContainer<P>(props: Props<P>) {
  // const [appConfig] = useAppConfig()
  const [isInBrowse, setInBrowse] = useState(false)
  useEffect(() => {
    setInBrowse(true)
  }, [])
  // useEffect(() => {
  //   const pathname = location.pathname
  //   let currMenu = getMenuByPath(pathname)
  //   document.title = currMenu?.name || appConfig.siteName
  // }, [appConfig.siteName])
  if (!isInBrowse) return <div>Loading</div>
  const LayoutComponent = withLayout(props.Component)
  return (
    <RecoilRoot>
      <React.Suspense fallback={<div>Loading...</div>}>
        <LoadPermission />
        {/* <ConfigProvider locale={zhCN}> */}
        <LayoutComponent {...props.pageProps} />
        {/* </ConfigProvider> */}
      </React.Suspense>
    </RecoilRoot>
  )
}
export default AppContainer
