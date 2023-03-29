import React, { useCallback, useEffect, useState } from 'react'
import { ConfigProvider, Spin } from 'antd'
import enUS from 'antd/locale/en_US'
import zhCN from 'antd/locale/zh_CN'
import dayjs from 'dayjs'
import 'dayjs/locale/zh-cn'
import withLayout from './withLayout'
import DocumentTitle from './DocumentTitle'
import { PageProps, PageContextProvider } from '@/views/PageContext'
import { OverlayContainer } from '@/components/overlay'
dayjs.locale('en')
export default function AppContainer<P = any>(props: PageProps<P>) {
  const [loading, setLoading] = useState(true)
  useEffect(() => {
    setLoading(false)
  }, [props.Component])

  if (loading) return <div></div>
  const LayoutComponent = withLayout(props.Component)
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
