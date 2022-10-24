import { Menu } from 'antd'
import { DataNode } from 'antd/es/tree'
export interface TreeEntity {
  id?: number
  parentId?: number
  name?: string
  level?: number
}
export type TreeNodeData<T extends TreeEntity> = T & DataNode

export function toCascaderData<T extends TreeEntity>(originData: T[]): TreeNodeData<T>[] {
  return toTreeData(originData)
}

export function toTreeData<T extends TreeEntity>(treeItems: T[]): TreeNodeData<T>[] {
  let treeData: TreeNodeData<T>[] = []
  let treeMap: Record<string, TreeNodeData<T>> = {}

  treeItems.forEach((item) => {
    let node = { ...item, title: item.name, value: item.id, key: item.id }
    if (!node.parentId || !treeMap[node.parentId]) {
      node.level = 1
      treeData.push(node)
    } else if (treeMap[node.parentId]) {
      let parent = treeMap[node.parentId]
      parent.children = parent.children || []
      parent.children.push(node)
      node.level = parent.level + 1
    }
    treeMap[node.id!] = node
  })
  return treeData
}

export function toMenuTree(menus?: Menu[]): DataNode[] {
  if (!menus) return []
  return menus.map((menu) => {
    return { title: menu.name, key: menu.path, children: toMenuTree(menu.children) }
  })
}
