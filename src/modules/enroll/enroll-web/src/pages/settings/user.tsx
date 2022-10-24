import React, {useCallback, useEffect, useMemo, useState} from 'react'
import adminApi, {User} from '@/api/admin'
import {ApiTable, FormItem, Form, overlay} from '@/components'
import {userTitles, userStatus} from './userData'
import AuthorizeRoles from './components/AuthorizeRoles'
import ResetPassword from './components/ResetPassword'

const UserEdit: ModalComponent<User> = (props) => {
  let {model: user} = props

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
UserEdit.defaultModalProps = (props) => {
  return {title: props.model?.id ? userTitles.update : userTitles.create}
}
const UserPage: PageComponent = () => {
  const onEditUser = useCallback(
    async (user?: User) => overlay.showForm(UserEdit, user),
    [],
  )
  const disabledUser = useCallback(async (user: User) => {
    if (await overlay.confirm('确定停用该该用户吗')) {
      await adminApi.users.disable(user.id)
    }
  }, [])

  return (
    <ApiTable
      titles={userTitles}
      toolBar={[
        {
          title: userTitles.create,
          buttonType: 'create',
          onClick: onEditUser,
        },
        {buttonType: 'export', title: '导出'},
        {buttonType: 'import', title: '导入', importApi: adminApi.users.getPageList},
      ]}
      api={adminApi.users.getPageList}
      tableTitle="用户列表"
      columns={[
        {valueType: 'index'},
        {dataIndex: 'account'},
        {dataIndex: 'name', operator: 'like'},
        {
          dataIndex: 'status',
          options: userStatus,
        },
        {
          width: 210,
          actions: [
            {onClick: onEditUser, title: '编辑'},
            {title: '授权角色', code: 'authorize', component: AuthorizeRoles},
            {title: '重置密码', component: ResetPassword},
            {
              onClick: disabledUser,
              title: '启用',
              hidden: (data) => !data.status,
            },
            {
              onClick: disabledUser,
              title: '禁用',
              hidden: (data) => !!data.status,
            },
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
