import React, { useState } from 'react'
import { BellOutlined, LogoutOutlined } from '@ant-design/icons'
import styles from './index.module.css'
import { useUser, logout } from '@/services/user'
import { Dropdown, Menu, Avatar } from 'antd'
const AvatarDropdown: React.FC = () => {
  const [user] = useUser()
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
  if (!user.isLogin) return null
  return (
    <Dropdown overlayClassName={styles.container} overlay={dropDown}>
      <span className={`${styles.account}`}>
        {/* <Avatar size="small" className={styles.avatar} src={currentUser.avatar} alt="avatar" /> */}
        <span className={`${styles.name} anticon`}>{user.nickName}</span>
      </span>
    </Dropdown>
  )
}
export default AvatarDropdown
