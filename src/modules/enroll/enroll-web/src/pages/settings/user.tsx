import React, { useMemo } from 'react'
import adminApi, { User, UserCreateDto, UserStatus, userStatusOptions } from '@/api/admin'
import { ApiTable, FormItem, Form, overlay } from '@/components'

import AuthorizeRoles from './components/AuthorizeRoles'
import ResetPassword from './components/ResetPassword'
import { FormControlItem } from '@/components/form/Form'

export const userLabels: Labels<User> = {
  name: '用户账号',
  password: '用户密码',
  nickName: '用户昵称',
  phone: '手机号码',
  email: '邮箱',
  status: '用户状态',

  disable: '禁用',
  create: '新增用户',
  update: '编辑用户',
  headImg: '用户头像',
  gender: '性别',
  authorizeRoles: '授权角色',
  export: '导出',
  initialPassword: '用户密码',
}

const userFormItems: FormControlItem<UserCreateDto | User>[] = [
  { name: 'name', control: 'input', required: true },
  { name: 'nickName', control: 'input', required: true },
  { name: 'phone', control: 'input', type: 'mobile', required: true },
]
const UserCreateModal: ModalComponent<UserCreateDto> = ({ model }) => {
  const data = { ...model, initialPassword: '123456' }
  return (
    <Form
      initialValues={data}
      api={adminApi.users.create}
      items={[
        { control: 'text', children: `用户初始密码为：${data.initialPassword}` },
        ...userFormItems,
      ]}></Form>
  )
}

const UserEditModal: ModalComponent<User> = (props) => {
  return <Form initialValues={props.model} api={adminApi.users.update} items={userFormItems} />
}

UserEditModal.modalProps = ({ model: user }) => {
  return { title: user.id ? userLabels.update : userLabels.create }
}
const UserPage: PageComponent = () => {
  // const disabledUser = useCallback(async (user: UserDto) => {
  //   if (await overlay.confirm('确定停用该该用户吗')) {
  //     await adminApi.users.disable(user.id)
  //   }
  // }, [])

  return (
    <ApiTable
      request={adminApi.users.search}
      tableTitle="用户列表"
      toolBar={[
        {
          actionName: 'create',
          actionType: 'create',
          component: UserCreateModal,
          type: 'primary',
        },
        { actionType: 'export', actionName: 'export' },
        // { buttonType: 'import', title: '导入',  },
      ]}
      columns={[
        { valueType: 'index' },
        { dataIndex: 'name', operator: 'like' },
        { dataIndex: 'nickName' },
        { dataIndex: 'phone', operator: 'like' },
        {
          dataIndex: 'status',
          options: userStatusOptions,
        },
        {
          width: 210,
          actions: [
            { title: '编辑', component: UserEditModal },
            { title: '授权角色', actionName: 'authorizeRoles', component: AuthorizeRoles },
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
  labels: userLabels,
  code: '000101',
}
export default UserPage
