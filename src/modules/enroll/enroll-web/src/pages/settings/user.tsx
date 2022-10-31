import React from 'react'
import adminApi, { UserDto, UserStatus, userStatusOptions } from '@/api/admin'
import { ApiTable, FormItem, Form, overlay } from '@/components'

import AuthorizeRoles from './components/AuthorizeRoles'
import ResetPassword from './components/ResetPassword'

export const userTitles = {
  account: '用户账号',
  password: '用户密码',
  name: '用户姓名',
  phone: '手机号码',
  email: '邮箱',
  status: '用户状态',
  update: '编辑',
  disable: '禁用',
  create: '新增用户',
  headImg: '用户头像',
  gender: '性别',
}

const UserEdit: ModalComponent<UserDto> = ({ model: user }) => {
  let api = user?.id ? adminApi.users.update : adminApi.users.create
  return (
    <Form
      initialValues={user}
      titles={userTitles}
      api={user?.id ? adminApi.users.update : adminApi.users.create}>
      {user?.id ? false : <FormItem.Text>用户初始密码为：123456</FormItem.Text>}
      <FormItem.Input name="account" readOnly={!!user?.id} required />
      <FormItem.Input name="name" required />
      <FormItem.Input name="phone" required type="mobile" />
    </Form>
  )
}
UserEdit.modalOptions = ({ model: user }) => {
  return { title: user?.id ? userTitles.update : userTitles.create }
}
const UserPage: PageComponent = () => {
  // const disabledUser = useCallback(async (user: UserDto) => {
  //   if (await overlay.confirm('确定停用该该用户吗')) {
  //     await adminApi.users.disable(user.id)
  //   }
  // }, [])

  return (
    <ApiTable
      titles={userTitles}
      toolBar={[
        {
          title: userTitles.create,
          buttonType: 'create',
          component: UserEdit,
        },
        { buttonType: 'export', title: '导出' },
        // { buttonType: 'import', title: '导入',  },
      ]}
      fetchApi={adminApi.users.query}
      tableTitle="用户列表"
      columns={[
        { valueType: 'index' },
        { dataIndex: 'account' },
        { dataIndex: 'phone', operator: 'like' },
        {
          dataIndex: 'status',
          options: userStatusOptions,
        },
        {
          width: 210,
          actions: [
            { title: '编辑', component: UserEdit },
            { title: '授权角色', code: 'authorize', component: AuthorizeRoles },
            { title: '重置密码', component: ResetPassword },
            // {
            //   onClick: disabledUser,
            //   title: '启用',
            //   hidden: (data) => !data.status,
            // },
            // {
            //   onClick: disabledUser,
            //   title: '禁用',
            //   hidden: (data) => !!data.status,
            // },
          ],
        },
      ]}
    />
  )
}
UserPage.options = {
  name: '用户管理',
  path: '/auth/settings/user',
}
export default UserPage
