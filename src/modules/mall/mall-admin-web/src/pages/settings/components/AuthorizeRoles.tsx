import React, {useState} from 'react'
import adminApi, {Role, User} from '@/api/admin'
import {FormItem, Form, Loading} from '@/components'
import {Card} from 'antd'
const AuthorizeRoles: ModalComponent<User> = (props) => {
  let user = props.model
  const [roleIds, setroleIds] = useState([] as number[])
  const [Roles, setRoles] = useState([] as Role[])
  const loadData = async () => {
    let [roleIds, Roles] = await Promise.all([
      adminApi.users.getUserRoleIds(user.id),
      adminApi.roles.getPageList({pageSize: -1}),
    ])
    setroleIds(roleIds)
    setRoles(Roles.items)
  }

  const saveApi = async (formData: any) => {
    console.warn(formData)

    return adminApi.users.authorizeRoles(user.id, formData.roleIds)
  }

  return (
    <Loading loadData={loadData}>
      <Form initialValues={{roleIds}} api={saveApi} noLabel>
        <Card title={`当前用户：${user.name || user.account}`}>
          <FormItem.CheckboxGroup name="roleIds" options={Roles} />
        </Card>
      </Form>
    </Loading>
  )
}

AuthorizeRoles.defaultModalProps = (props) => {
  return {title: '角色授权', width: 800}
}
export default AuthorizeRoles
