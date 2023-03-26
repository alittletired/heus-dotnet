import React, {useCallback, useState} from 'react'
import adminApi, {SysOption, SysOptionGroup} from '@/api/admin'
import {ApiTable, Form, FormItem, overlay} from '@/components'
import ApiSideTree from '@/components/tree/ApiSideTree'
import ActionAnchor from '@/components/action/ActionAnchor'

const titles = {
  text: '单项文本',
  value: '单项值',
  desc: '单项描述',
  groupId: '分组',
  create: '新增单项',
  update: '修改单项',
  createGroup: '新增分组',
  updateGroup: '修改分组',
}
const SysOptionGroupEdit: ModalComponent<SysOptionGroup> = (props) => {
  return (
    <Form initialValues={props.model} api={adminApi.sysOptions.saveGroup}>
      <FormItem.Input name="name" label="分组名" required />
    </Form>
  )
}
SysOptionGroupEdit.defaultModalProps = (data) => {
  return {title: data?.model?.id ? titles.updateGroup : titles.createGroup}
}

const SysOptionEdit: ModalComponent<SysOption> = (props) => {
  return (
    <Form
      titles={titles}
      initialValues={props.model}
      api={props.model?.id ? adminApi.sysOptions.update : adminApi.sysOptions.create}>
      <FormItem.Select
        name="groupId"
        disabled
        options={adminApi.sysOptions.getAllGroups}
      />
      <FormItem.Input name="text" required />
      <FormItem.Input name="value" required />
    </Form>
  )
}
SysOptionEdit.defaultModalProps = (props) => {
  return {title: props.model?.id ? titles.update : titles.create}
}
const SysOptionPage: PageComponent = () => {
  const deleteOption = useCallback(async (data: SysOption) => {
    if (await overlay.deleteConfirm()) {
      return adminApi.sysOptions.delete(data)
    }
  }, [])
  const deleteOptionGroup = useCallback(async (data: SysOptionGroup) => {
    if (await overlay.deleteConfirm()) {
      await adminApi.sysOptions.deleteGroup(data.id)
    }
  }, [])

  const [data, setData] = useState<SysOption>({})
  return (
    <ApiSideTree
      api={adminApi.sysOptions.getAllGroups}
      title="字典分组"
      toolBar={[{title: '新增分组', component: SysOptionGroupEdit}]}
      onSelect={(data) => setData({groupId: data.id})}
      nodeActions={[
        {icon: 'edit', title: '编辑', component: SysOptionGroupEdit},
        {icon: 'delete', title: '删除', onClick: deleteOptionGroup},
      ]}>
      <ApiTable
        data={data}
        onApiBefore={(data) => {
          return !!data.groupId
        }}
        titles={titles}
        api={adminApi.sysOptions.getPageList}
        tableTitle="数据字典列表"
        toolBar={[
          {
            title: titles.create,
            component: SysOptionEdit,
            buttonType: 'create',
            hidden: (data) => !data.groupId,
          },
        ]}
        columns={[
          {valueType: 'index'},
          {dataIndex: 'text'},
          {dataIndex: 'value'},
          {dataIndex: 'desc'},
          {
            actions: [
              {
                component: SysOptionEdit,
                title: '编辑',
              },

              {
                title: '删除',
              },
            ],
          },
        ]}
      />
    </ApiSideTree>
  )
}
SysOptionPage.options = {name: '数据字典', path: '/auth/system-option'}
export default SysOptionPage
