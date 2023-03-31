import React, { useCallback, useEffect, useState } from 'react'
import { ConfigProvider } from 'antd'
import withLayout from './layouts/withLayout'
import DocumentTitle from './layouts/DocumentTitle'
import { PageProps, PageContextProvider } from '@/views/PageContext'
import { OverlayContainer } from '@/components/overlay'
export default function AppContainer<P = any>(props: PageProps<P>) {
  const [loading, setLoading] = useState(true)
  useEffect(() => {
    setLoading(false)
  }, [props.Component])

  if (loading) return <div></div>
  const LayoutComponent = withLayout(props.Component, props.Component.options?.layout)
  return (
    <ConfigProvider locale={zhCN}>
      <DocumentTitle />
      <PageContextProvider {...props}>
        <LayoutComponent {...props.pageProps} />
        <OverlayContainer />
      </PageContextProvider>
    </ConfigProvider>
  )
}
