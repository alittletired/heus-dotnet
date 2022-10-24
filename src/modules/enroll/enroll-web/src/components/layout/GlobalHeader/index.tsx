import React from 'react'
import {Layout} from 'antd'

import './index.css'
import {useAppConfig} from '@/layout/appConfig'
import NoticeIcon from './NoticeIcon'
import AvatarDropdown from './AvatarDropdown'
export const renderLogo = (logo: React.ReactNode): React.ReactNode => {
  if (typeof logo === 'string') {
    return <img src={logo} alt="logo" />
  }

  return logo
}
const GlobalHeader = () => {
  const appConfig = useAppConfig()
  const menuDom = <></>
  return (
    <>
      <Layout.Header
        style={{height: '48px', lineHeight: '48px', background: 'transparent'}}
      />
      <Layout.Header className="global-header-fixed">
        <div className="global-header">
          <div className="global-header-logo">
            <a href="/">
              <img src={appConfig.logo} alt="logo" />
              <h1>{appConfig.siteName}</h1>
            </a>
          </div>
          <div style={{flex: '1 1 0%'}}>{menuDom}</div>
          <div className="global-header-right">
            <div className="global-header-right-action">{/* <NoticeIcon /> */}</div>
            <div className="global-header-right-action">
              <AvatarDropdown />
            </div>
          </div>
        </div>
      </Layout.Header>
    </>
  )
}
export default GlobalHeader
