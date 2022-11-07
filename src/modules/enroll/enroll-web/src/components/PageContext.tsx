import { useAppConfig } from '@/layouts/appConfig'
import authState, { useAuth } from '@/services/auth'
import useRouter from '@/services/router'
import { useUser } from '@/services/user'
import { Spin } from 'antd'

import React, { PropsWithChildren, useCallback } from 'react'
export interface PageContext {
  labels?: Record<string, string>
}
export interface PageProps<P = any> {
  pageProps: P
  Component: PageComponent<P>
}
const PageContext = React.createContext({} as PageContext)
export const usePageContext = () => React.useContext(PageContext)

export const PageContextProvider: React.FC<PageProps & PropsWithChildren> = (props) => {
  const labels = props.Component.options?.labels
  const router = useRouter()
  const [user] = useUser()
  const [appConfig] = useAppConfig()
  const auth = useAuth()
  if (!(user.isLogin && auth.hasRight('view')) && router.asPath !== appConfig.loginUrl) {
    console.warn('redirect for login:', router.asPath, appConfig.loginUrl)
    router.replace(appConfig.loginUrl)
    return <Spin />
  }
  return <PageContext.Provider value={{ labels }}>{props.children}</PageContext.Provider>
}

export default PageContext
