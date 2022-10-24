import menus from '@/config/menus'
export interface Menu {
  name: string
  key?: string //自动生成
  icon?: string
  path: string
  type: number
  children?: Menu[]
}

const menuMap = new Map<string, Menu>()
function initMenuMap(menuArr: Menu[]) {
  if (!menuArr) return
  for (let menu of menuArr) {
    menuMap.set(menu.path, menu)
    menu.children && initMenuMap(menu.children)
  }
}
initMenuMap(menus)

export const getMenuByPath = (path: string) => {
  return menuMap.get(path)
}
export const getOpenKeys = (selectKey: string) => {
  if (!selectKey) return []

  let openKeys = []
  // let lastIndex = selectKey.lastIndexOf('.')
  let index = selectKey.indexOf('.')
  while (index !== -1) {
    openKeys.push(selectKey.substring(0, index))
    index = selectKey.indexOf('.', index + 1)
  }

  return openKeys
}
