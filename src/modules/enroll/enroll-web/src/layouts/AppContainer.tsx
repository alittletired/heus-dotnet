import React, { useEffect, useState } from 'react'
import { ConfigProvider } from 'antd'
// import zhCN from 'antd/es/locale/zh_CN'
import { RecoilRoot } from 'recoil'
import withLayout from './withLayout'
import DocumentTitle from './DocumentTitle'
import { RecoilAsyncState } from '@/services'

interface Props<P> {
  pageProps: P
  Component: React.ComponentType<P>
}

export default function AppContainer<P = any>(props: Props<P>) {
  // const [appConfig] = useAppConfig()
  const [isInBrowse, setInBrowse] = useState(false)
  useEffect(() => {
    setInBrowse(true)
  }, [])

  if (!isInBrowse) return <div>Loading</div>
  const LayoutComponent = withLayout(props.Component)
  return (
    <RecoilRoot>
      {/* <ConfigProvider locale={zhCN}> */}
      <RecoilAsyncState>
        <DocumentTitle />
        <LayoutComponent {...(props.pageProps as any)} />
      </RecoilAsyncState>
      {/* </ConfigProvider> */}
    </RecoilRoot>
  )
}
