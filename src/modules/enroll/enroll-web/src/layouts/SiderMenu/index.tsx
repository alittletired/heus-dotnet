import React, { useMemo, useState, useEffect } from 'react'
import { Layout, Menu as AntdMenu } from 'antd'
import { useAppConfig } from '@/layouts/appConfig'
import { findMenusByPath, Menu, useUserMenu } from '@/services/menu'
import { ItemType } from 'antd/es/menu/hooks/useItems'
import { Link } from '@/components'
import useRouter from '@/services/router'
const { Sider } = Layout

const SiderMenu: React.FC = (props) => {
  const router = useRouter()
  const [appContext] = useAppConfig()
  const userMenu = useUserMenu()
  const menuItems: ItemType[] = useMemo(() => {
    function getMenus(userMenu: Menu[]) {
      let menus: ItemType[] = []
      if (!userMenu || userMenu.length === 0) return []
      for (let menu of userMenu) {
        if (menu.children) {
          var menuItem = { label: menu.name, key: menu.path, children: getMenus(menu.children) }
          menus.push(menuItem)
        } else {
          menus.push({
            key: menu.path,
            label: (
              <Link href={menu.path}>
                <span>{menu.name}</span>
              </Link>
            ),
          })
        }
      }
      return menus
    }

    let menus = getMenus(userMenu)

    return menus
  }, [userMenu])

  let [selectedKeys, setSelectedKeys] = useState([] as string[])
  let [openKeys, setOpenKeys] = useState([] as string[])
  useEffect(() => {
    let menus = findMenusByPath(router.pathname)
    if (!menus || menus.length == 0) {
      return
    }
    setSelectedKeys([menus[menus.length - 1].path])
    setOpenKeys(menus.map((m) => m.path))
  }, [router.pathname])

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
