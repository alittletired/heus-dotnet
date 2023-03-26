import { ActionDto, ResourceDto } from '@/api/admin'
import menus from '@/config/menus'
import { useMemo, useState } from 'react'
import authState, { useAuth } from './auth'
export interface Menu {
  name: string
  icon?: string
  path: string
  children?: Menu[]
  code?: string
}

export const useMenu = () => {
  const { hasActionRight } = useAuth()

  const userMenus = useMemo(() => {
    function getUserMenus(menus: Menu[], parentMenu?: Menu) {
      let userMenu: Menu[] = []
      if (!menus.length) return userMenu
      for (let menu of menus) {
        const { children, ...item } = menu
        if (menu.children) {
          const children = getUserMenus(menu.children, menu)
          if (children.length) {
            userMenu.push(item)
          }
        } else if (hasActionRight(item.path, 'view')) {
          userMenu.push(item)
        }
      }
      return useMenu
    }
    const userMenus = getUserMenus(menus)
    return userMenus
  }, [hasActionRight])
  return { menus }
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
