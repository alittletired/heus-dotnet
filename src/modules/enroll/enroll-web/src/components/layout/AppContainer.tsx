import React, { PropsWithChildren, useEffect, useMemo } from 'react'
import ProtectedLayout from './ProtectedLayout'
import { ConfigProvider } from 'antd'
import zhCN from 'antd/es/locale/zh_CN'
import { AppConfig, AppConfigProvider } from './appConfig'
import { useAuth, usePermission } from '@/services/auth'
import { getMenuByPath } from '@/services/menu'
import { RecoilRoot } from 'recoil'
interface Props extends AppConfig {}
const AppContainer: React.FC<AppConfig & PropsWithChildren> = (props) => {
  const [auth] = useAuth()
  const { hasPermission } = usePermission()

  useEffect(() => {
    const pathname = location.pathname
    let currMenu = getMenuByPath(pathname)
    document.title = currMenu?.name || props.siteName
  }, [props.siteName])

  return (
    <RecoilRoot>
      <AppConfigProvider {...props}>
        <ConfigProvider locale={zhCN}>{props.children}</ConfigProvider>
      </AppConfigProvider>
    </RecoilRoot>
  )
}
export default AppContainer
