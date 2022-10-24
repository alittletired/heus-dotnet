import React, {useMemo, useState, useEffect} from 'react'
import {Layout, Menu as AntdMenu} from 'antd'
import {Link, useLocation} from 'react-router-dom'
import './index.css'
import {useAppConfig} from '@/layout/appConfig'
import {getMenuByPath, getOpenKeys} from '@/services/menu'
import {useAuth, usePermission} from '@/services/auth'
import {ItemType} from 'antd/lib/menu/hooks/useItems'
const {Sider} = Layout

const SiderMenu: React.FC<{menus: Menu[]}> = (props) => {
  const {pathname} = useLocation()
  const {hasPermission} = usePermission()
  const appContext = useAppConfig()

  const menuItems: ItemType[] = useMemo(() => {
    function getUserMenu(menus: Menu[], parentMenu?: Menu) {
      let userMenu: ItemType[] = []
      if (!menus || menus.length === 0) return userMenu
      for (let menu of menus) {
        if (menu.children) {
          const children = getUserMenu(menu.children, menu)
          if (children.length) {
            const item = {label: menu.name, key: menu.key, children}
            userMenu.push(item)
          }
        } else if (hasPermission(menu.path)) {
          const item = {
            key: menu.key,
            label: (
              <Link to={menu.path}>
                <span>{menu.name}</span>
              </Link>
            ),
          }
          userMenu.push(item)
        }
      }
      return userMenu
    }

    let menus = getUserMenu(appContext.menus)

    return menus
  }, [appContext.menus, hasPermission])

  let [selectedKeys, setSelectedKeys] = useState([])
  let [openKeys, setOpenKeys] = useState([])
  useEffect(() => {
    let menu = getMenuByPath(pathname)

    if (!menu) {
      return
    }
    let selectedKey = menu.key

    setSelectedKeys([selectedKey])
    setOpenKeys(getOpenKeys(selectedKey))
  }, [pathname])

  return (
    <Sider
      trigger={null}
      collapsible
      theme="light"
      className="sider-fixed"
      collapsed={appContext.collapsed}>
      <AntdMenu
        mode="inline"
        selectedKeys={selectedKeys}
        openKeys={openKeys}
        items={menuItems}
        onOpenChange={(value: any) => setOpenKeys(value)}></AntdMenu>
    </Sider>
  )
}
export default SiderMenu
