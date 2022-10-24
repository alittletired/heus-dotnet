import { Menu } from '@/services/menu'
import React, { useState, useCallback, PropsWithChildren } from 'react'

export interface AppConfig {
  menus: Menu[]
  logo?: string
  siteName: string
  loginUrl: string
  toggle?: () => void
  collapsed?: boolean
}

const AppConfig = React.createContext<AppConfig>({} as any)
export const AppConfigProvider: React.FC<PropsWithChildren<AppConfig>> = (props) => {
  const [collapsed, setCollapse] = useState(false)
  let { children, ...rest } = props
  const toggle = useCallback(() => {
    setCollapse((collapsed) => !collapsed)
  }, [])
  return <AppConfig.Provider value={{ collapsed, toggle, ...rest }}>{children}</AppConfig.Provider>
}
export default AppConfig
export const useAppConfig = () => React.useContext(AppConfig)
