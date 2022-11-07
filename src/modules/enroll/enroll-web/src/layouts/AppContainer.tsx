import React, { useCallback, useEffect, useState } from 'react'
import { ConfigProvider } from 'antd'
// import zhCN from 'antd/es/locale/zh_CN'
import withLayout from './withLayout'
import DocumentTitle from './DocumentTitle'
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
    <>
      <DocumentTitle />
      <PageContextProvider {...props}>
        <LayoutComponent {...(props.pageProps as any)} />
      </PageContextProvider>
    </>
  )
}
