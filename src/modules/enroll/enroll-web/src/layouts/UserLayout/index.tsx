import React, { PropsWithChildren } from 'react'
import { Link, Image } from '@/components'
import { useAppConfig } from '../appConfig'
import styles from './index.module.css'
type LayoutProps = React.PropsWithChildren<{
  containerClass?: string
}>

const UserLayout = (props: LayoutProps) => {
  const [appConfig] = useAppConfig()
  return (
    <div className={styles.container}>
      <div className={styles.lang}></div>
      <div className={styles.content}>
        <div className={styles.top}>
          <div className={styles.header}>
            <Link href="/">
              <Image alt="logo" className={styles.logo} src="/favicon.png" />
              <span className={styles.title}>{appConfig.siteName}</span>
            </Link>
          </div>
          <div className={styles.desc}></div>
        </div>
        <div className={props.containerClass}>{props.children}</div>
      </div>
    </div>
  )
}

export default UserLayout
