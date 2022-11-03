import React, { useCallback, useEffect, useState } from 'react'
import { ConfigProvider } from 'antd'
// import zhCN from 'antd/es/locale/zh_CN'
import { RecoilRoot } from 'recoil'
import withLayout from './withLayout'
import DocumentTitle from './DocumentTitle'
import { RecoilAsyncState } from '@/services'
import PageContext from '@/components/PageContext'

interface Props<P> {
  pageProps: P
  Component: React.ComponentType<P>
}

export default function AppContainer<P = any>(props: Props<P>) {
  // const [appConfig] = useAppConfig()
  const [loading, setLoading] = useState(true)
  useEffect(() => {
    setLoading(false)
  }, [props.Component])
  const doLoading = useCallback(async (func: () => Promise<any>) => {
    try {
      setLoading(true)
      await func()
    } catch (e) {
      console.warn(e)
    } finally {
      setLoading(false)
    }
  }, [])
  var labels = (props.Component as PageComponent).options?.labels
  if (loading) return <div>Loading</div>
  const LayoutComponent = withLayout(props.Component)
  return (
    <RecoilRoot>
      {/* <ConfigProvider locale={zhCN}> */}
      <RecoilAsyncState>
        <DocumentTitle />
        <PageContext.Provider value={{ labels, loading, doLoading }}>
          <LayoutComponent {...(props.pageProps as any)} />
        </PageContext.Provider>
      </RecoilAsyncState>
      {/* </ConfigProvider> */}
    </RecoilRoot>
  )
}
