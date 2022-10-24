const menus = [
  {
    name: '报名管理',
    key: '报名管理',
    type: 1,
    path: '/enroll',
    children: [{ name: '报名审核', key: '报名管理.报名审核', type: 2, path: '/enroll/approve' }],
  },
  {
    name: '系统设置',
    key: '系统设置',
    type: 1,
    path: '/settings',
    children: [
      { name: '用户管理', key: '系统设置.用户管理', type: 2, path: '/settings/user' },
      { name: '菜单管理', key: '系统设置.菜单管理', type: 2, path: '/settings/menu' },
      { name: '角色管理', key: '系统设置.角色管理', type: 2, path: '/settings/role' },
      {
        name: '数据字典',
        key: '系统设置.数据字典',
        type: 2,
        path: '/settings/system-option',
      },
      {
        name: '系统参数',
        key: '系统设置.系统参数',
        type: 2,
        path: '/settings/system-param',
      },
    ],
  },
]
export default menus
