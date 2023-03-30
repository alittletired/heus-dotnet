import { useGlobalConfig } from '@/services/globalConfig'
import useRouter from '@/services/router'
import user from '@/services/user'
import { Spin } from 'antd'

import React, { PropsWithChildren, useCallback } from 'react'
export interface PageContext {
  options: PageOptions
}
export interface PageProps<P = any> {
  pageProps: P
  Component: PageComponent<P>
}
const PageContext = React.createContext({} as PageContext)
export const usePageContext = () => React.useContext(PageContext)

export const PageContextProvider: React.FC<PageProps & PropsWithChildren> = (props) => {
  const router = useRouter()
  const [userState] = user.useState()
  const [globalConfig] = useGlobalConfig()
  if (!userState.isLogin && !router.asPath.startsWith(globalConfig.loginUrl)) {
    console.warn('redirect for login:', router, globalConfig.loginUrl)
    router.replace(globalConfig.loginUrl + '?redirect=' + encodeURIComponent(router.asPath))
    return <Spin />
  }
  return (
    <PageContext.Provider value={{ options: props.Component.options || {} }}>
      {props.children}
    </PageContext.Provider>
  )
}

export default PageContext
