import React from 'react'
import { Layout } from 'antd'
import styles from './index.module.css'
import { useAppConfig } from '@/layouts/appConfig'
import NoticeIcon from './NoticeIcon'
import AvatarDropdown from './AvatarDropdown'
import { Link, Image } from '@/components'

const GlobalHeader = () => {
  const [appConfig] = useAppConfig()
  const menuDom = <></>
  return (
    <>
      <Layout.Header style={{ height: '48px', lineHeight: '48px', background: 'transparent' }} />
      <Layout.Header className={styles.headerFixed}>
        <div className={styles.header}>
          <div className={styles.headerLogo}>
            <Link href="/">
              <Image src={appConfig.loginUrl} alt="logo" />
              <h1>{appConfig.siteName}</h1>
            </Link>
          </div>
          <div style={{ flex: '1 1 0%' }}>{menuDom}</div>
          <div className={styles.headerRight}>
            <div className={styles.headerRightAction}>{/* <NoticeIcon /> */}</div>
            <div className={styles.headerRightAction}>
              <AvatarDropdown />
            </div>
          </div>
        </div>
      </Layout.Header>
    </>
  )
}
export default GlobalHeader
