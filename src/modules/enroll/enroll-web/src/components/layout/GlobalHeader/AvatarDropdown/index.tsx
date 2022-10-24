import React, {useState} from 'react'
import {BellOutlined, LogoutOutlined} from '@ant-design/icons'
import styles from './index.module.less'
import {useAuth,logout} from '@/services/auth'
import {Dropdown, Menu, Avatar} from 'antd'
const AvatarDropdown: React.FC = () => {
  const [auth] = useAuth()
  const onMenuClick = (key: any) => {
    console.warn('key', key)

    if (key.key === 'logout') {
      logout()
    }
  }
  const dropDown = (
    <Menu className={styles.menu} selectedKeys={[]} onClick={onMenuClick}>
      {/* {menu && (
				<Menu.Item key="center">
					<UserOutlined />
					个人中心
				</Menu.Item>
			)} */}
      <Menu.Divider />
      <Menu.Item key="logout">
        <LogoutOutlined />
        退出登录
      </Menu.Item>
    </Menu>
  )
  if (!auth.isLogin) return null
  return (
    <Dropdown overlayClassName={styles.container} overlay={dropDown}>
      <span className={`${styles.account}`}>
        {/* <Avatar size="small" className={styles.avatar} src={currentUser.avatar} alt="avatar" /> */}
        <span className={`${styles.name} anticon`}>
          {auth.name || auth.account}
        </span>
      </span>
    </Dropdown>
  )
}
export default AvatarDropdown
