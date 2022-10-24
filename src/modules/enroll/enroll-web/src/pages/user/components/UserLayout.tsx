import React from 'react'

import { Link } from '@/utils/routerUtils'
type LayoutProps = React.PropsWithChildren<{
  containerClass?: string
}>
const UserLayout: React.FC<LayoutProps> = (props) => {
  return (
    <div className={styles.container}>
      <div className={styles.lang}></div>
      <div className={styles.content}>
        <div className={styles.top}>
          <div className={styles.header}>
            <Link to="/">
              <Image alt="logo" className={styles.logo} src="/favicon.png" />
              <span className={styles.title}>{config.siteName}</span>
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
