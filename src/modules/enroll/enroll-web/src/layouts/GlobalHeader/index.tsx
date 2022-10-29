import React from 'react'
import { Layout } from 'antd'
import { useAppConfig } from '@/layouts/appConfig'
import AvatarDropdown from './AvatarDropdown'
import { Link, Image } from '@/components'

const GlobalHeader = () => {
  const [appConfig] = useAppConfig()
  const menuDom = <></>
  return (
    <>
      <Layout.Header className="global-header-stub" />
      <Layout.Header className="global-header-fixed">
        <div className="global-header ">
          <div className="global-header-logo">
            <Link href="/">
              {/* <Image src={appConfig.loginUrl} alt="logo" /> */}
              <h1>{appConfig.siteName}</h1>
            </Link>
          </div>
          <div style={{ flex: '1 1 0%' }}>{menuDom}</div>
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
