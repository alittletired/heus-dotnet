import React, {useState} from 'react'
import adminApi, {Role, Resource} from '@/api/admin'
import {FormItem, Form, Loading} from '@/components'
import withFormItem from '@/components/form/withFormItem'
import TreeCheckGroup from './TreeCheckGroup'
import {Card} from 'antd'
const ItemTreeCheckGroup = withFormItem(TreeCheckGroup)
const AuthorizeActions: ModalComponent<Role> = (props) => {
  const [actionIds, setActionIds] = useState([] as number[])
  const [actions, setActions] = useState([] as Resource[])
  const loadData = async () => {
    let [actionIds, actions] = await Promise.all([
      adminApi.roles.getActionIds(props.model.id),
      adminApi.resources.pb.getList(),
    ])
    setActionIds(actionIds)
    setActions(actions)
  }

  const saveApi = async (formData: any) => {
    return adminApi.roles.authorizeActions(props.model.id, formData.actionIds)
  }

  return (
    <Loading loadData={loadData}>
      <Form initialValues={{actionIds}} api={saveApi} noLabel>
        <Card title={`角色:${props.model.name}`}>
          <ItemTreeCheckGroup name="actionIds" treeData={actions} />
        </Card>
      </Form>
    </Loading>
  )
}

AuthorizeActions.defaultModalProps = (props) => {
  return {title: '角色授权', width: 800}
}
export default AuthorizeActions
