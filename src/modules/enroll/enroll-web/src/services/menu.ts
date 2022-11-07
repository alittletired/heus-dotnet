import menus from '@/config/menus'
import authState, { AuthState, useAuth } from './auth'
export interface Menu {
  name: string
  icon?: string
  path: string
  type: number
  children?: Menu[]
  code?: string
}

export const useMenu = () => {
  const auth = useAuth()
  const getUserMenu = () => {}
  return { getUserMenu }
}
function getUserMenu(menus: Menu[], auth: AuthState, parentMenu?: Menu) {
  let userMenu: Menu[] = []
  if (!menus.length) return userMenu
  for (let menu of menus) {
    if (menu.children) {
      const children = getUserMenu(menu.children, auth, menu)
      if (children.length) {
        const item = { ...menu, children }
        userMenu.push(item)
      }
    } else if (auth.hasRight('view')) {
      userMenu.push(menu)
    }
  }

  return userMenu
}
export function findMenusByPath(path?: string) {
  function recursionFind(menus?: Menu[], path?: string): Menu[] | undefined {
    if (!path) return
    if (!menus) return
    for (const menu of menus) {
      if (!menu) continue
      if (menu.children) {
        const subMenus = recursionFind(menu.children, path)
        if (subMenus) return [menu, ...subMenus]
      }
      if (menu.path === path) {
        return [menu]
      }
    }
    return
  }
  return recursionFind(menus, path)
}
