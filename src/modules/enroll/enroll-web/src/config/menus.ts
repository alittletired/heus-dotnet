import { ResourceDto } from '@/api/admin'

const menus: ResourceDto[] = [
  {
    name: '报名管理',
    code: '0002',
    path: '/enroll',
    children: [{ name: '报名审核', code: '000201', path: '/enroll/approve' }],
  },
  {
    name: '系统设置',
    code: '0001',
    path: '/settings',
    children: [
      { name: '用户管理', code: '000101', path: '/settings/user' },
      { name: '菜单管理', code: '000102', path: '/settings/menu' },
      { name: '角色管理', code: '000103', path: '/settings/role' },
      {
        name: '数据字典',
        code: '000104',
        path: '/settings/system-option',
      },
      {
        name: '系统参数',
        code: '000105',
        path: '/settings/system-param',
      },
    ],
  },
]
export default menus
