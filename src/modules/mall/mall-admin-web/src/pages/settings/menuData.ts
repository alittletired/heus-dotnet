import {Resource} from '@/api/admin'
export const menuLabels: Record<keyof Resource | string, string> = {
  name: '名称',
  icon: '图标',
  path: '路径',
  type: '类型',
  actionCode: '权限编码',

  menuGroup: '菜单组',
  create: '新增菜单',
  update: '修改菜单',
}

export const menuTypes = [
  {value: 0, title: '模块'},
  {value: 1, title: '菜单组'},
  {value: 2, title: '菜单'},
  {value: 3, title: '动作点'},
]
export const editMenuTypes = menuTypes.filter((t) => t.value === 1 || t.value === 2)
