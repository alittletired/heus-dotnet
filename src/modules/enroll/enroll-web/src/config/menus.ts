export interface Menu {
  name: string
  type: number
  code: string
  path: string
  children?: Menu[]
  sort?: number
}

const menus: Menu[] = [
  {
    name: '报名管理',
    type: 1,
    code: '0002',
    path: '/enroll',
    sort: 1,
    children: [{ name: '报名审核', code: '000201', type: 2, path: '/enroll/approve' }],
  },
  {
    name: '系统设置',
    type: 1,
    code: '0001',
    sort: 2,
    path: '/settings',
    children: [
      { name: '用户管理', code: '000101', type: 2, path: '/settings/user' },
      { name: '菜单管理', code: '000102', type: 2, path: '/settings/menu' },
      { name: '角色管理', code: '000103', type: 2, path: '/settings/role' },
      {
        name: '数据字典',
        type: 2,
        code: '000104',
        path: '/settings/system-option',
      },
      {
        name: '系统参数',
        type: 2,
        code: '000105',
        path: '/settings/system-param',
      },
    ],
  },
]
export default menus
