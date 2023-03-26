import React, { useCallback } from 'react'
import adminApi, { Role } from '@/api/admin'
import { ApiTable, Form, FormItem, overlay } from '@/components'
import AuthorizeActionForm from './components/AuthorizeActionForm'

const roleLables: Labels<Role> = {
  name: '角色名',
  remarks: '角色描述',
  create: '新增角色',
  update: '修改角色',
}
const RoleEdit: ModalComponent<Role> = (props) => {
  return (
    <Form
      initialValues={props.model}
      request={props.model?.id ? adminApi.roles.update : adminApi.roles.create}>
      <FormItem.Input name="name" required />
      <FormItem.Input name="remarks" />
    </Form>
  )
}
RoleEdit.modalProps = (props) => {
  return { title: props.model.id ? roleLables.update : roleLables.create }
}
const RolePage: PageComponent = () => {
  const deleteRole = useCallback(async (data: Role) => {
    if (await overlay.deleteConfirm()) {
      return adminApi.roles.delete(data.id)
    }
  }, [])

  return (
    <ApiTable
      titles={roleLables}
      request={adminApi.roles.search}
      tableTitle="角色列表"
      toolBar={[
        {
          title: roleLables.create,
          component: RoleEdit,
          actionName: 'create',
        },
      ]}
      columns={[
        { valueType: 'index' },
        { dataIndex: 'name', operator: 'like' },

        { dataIndex: 'remarks' },
        {
          actions: [
            {
              component: RoleEdit,
              title: '编辑',
            },
            {
              component: AuthorizeActionForm,
              title: '授权',
            },
            {
              onClick: deleteRole,
              title: '删除',
              hidden: (data) => data.isBuildIn,
            },
          ],
        },
      ]}
    />
  )
}
RolePage.options = { name: '角色管理', labels: roleLables, code: '000103' }
export default RolePage
