import React, { useState } from 'react'
import { BellOutlined, LogoutOutlined } from '@ant-design/icons'
import { useUser, logout } from '@/services/user'
import { Dropdown, Menu, Avatar, MenuProps } from 'antd'
const AvatarDropdown: React.FC = () => {
  const [user] = useUser()
  const onMenuClick = (key: any) => {
    console.warn('key', key)

    if (key.key === 'logout') {
      logout()
    }
  }
  const menu: MenuProps = {
    onClick: onMenuClick,
    items: [
      { key: 'divider', label: <Menu.Divider /> },
      {
        key: 'logout',
        label: (
          <>
            <LogoutOutlined />
            退出登录
          </>
        ),
      },
    ],
  }

  if (!user.isLogin) return null
  return (
    <Dropdown menu={menu}>
      <span>
        {/* <Avatar size="small" className={styles.avatar} src={currentUser.avatar} alt="avatar" /> */}
        <span className={`anticon`}>{user.nickName}</span>
      </span>
    </Dropdown>
  )
}
export default AvatarDropdown
