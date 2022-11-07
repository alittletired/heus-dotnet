import React, { useCallback, useEffect, useState } from 'react'
import { ConfigProvider } from 'antd'
// import zhCN from 'antd/es/locale/zh_CN'
import { RecoilRoot } from 'recoil'
import withLayout from './withLayout'
import DocumentTitle from './DocumentTitle'
import { RecoilAsyncState } from '@/services'
import { PageProps, PageContextProvider } from '@/components/PageContext'

export default function AppContainer<P = any>(props: PageProps<P>) {
  // const [appConfig] = useAppConfig()
  const [loading, setLoading] = useState(true)
  useEffect(() => {
    setLoading(false)
  }, [props.Component])

  if (loading) return <div>Loading</div>
  const LayoutComponent = withLayout(props.Component)
  return (
    <RecoilRoot>
      {/* <ConfigProvider locale={zhCN}> */}
      <RecoilAsyncState>
        <DocumentTitle />
        <PageContextProvider {...props}>
          <LayoutComponent {...(props.pageProps as any)} />
        </PageContextProvider>
      </RecoilAsyncState>
      {/* </ConfigProvider> */}
    </RecoilRoot>
  )
}
