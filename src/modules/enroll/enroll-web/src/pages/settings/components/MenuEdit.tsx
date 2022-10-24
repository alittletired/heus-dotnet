import React, {useCallback, useState, useMemo, useRef} from 'react'
import {Card, Table} from 'antd'
import adminApi, {Resource} from '@/api/admin'
import {ApiTable, FormItem, Form} from '@/components'
import EditTable from '@/components/EditTable'
import {menuLabels, editMenuTypes} from '../menuData'
type TreeResource = Resource & {children?: TreeResource[]}

function getMenuGroups(menus: TreeResource[]) {
  return menus
    .filter((menu) => menu.type === 1)
    .map((menu) => {
      let {id, name, type, children, ...rest} = menu
      return {...rest, value: id, type, label: name, children}
    })
}

const MenuEdit: ModalComponent<Resource, {menus: TreeResource[]}> = (props) => {
  const [resource, setResource] = useState({type: 2, ...props.model})
  const disabled = !!resource?.id
  const parentItem = useMemo(() => {
    const treeData = getMenuGroups(props.menus)
    if (resource.type === 0) return null
    return (
      <FormItem.TreeSelect
        required
        treeData={treeData}
        label={menuLabels.menuGroup}
        name="parentId"
      />
    )
  }, [resource, props.menus])
  const changeType = useCallback((type: any) => {
    setResource((res) => ({...res, type}))
  }, [])
  return (
    <Form
      initialValues={resource}
      onSuccess={(data) => {
        if (data.id) return
        setResource((res) => ({...res, ...data}))
        return false
      }}
      api={resource?.id ? adminApi.roles.update : adminApi.roles.create}>
      <FormItem.RadioGroup
        onChange={changeType}
        label={menuLabels.type}
        name="type"
        options={editMenuTypes}
        disabled={disabled}
        required
      />
      {parentItem}
      <FormItem.Input label={menuLabels.name} name="name" required />
      <FormItem.Input label={menuLabels.path} name="path" required />
      {/* <FormItem.Input label={menuLabels.icon} name="icon" /> */}
      {resource.id && (
        <EditTable
          api={adminApi.resources.getPageList}
          deleteApi={adminApi.resources.delete}
          updateApi={adminApi.resources.update}
          createApi={adminApi.resources.create}
          pagination={{pageSize: -1}}
          tableTitle="权限列表"
          data={{parentId: resource.id, type: 3, actionCode: {$neq: 'view'}}}
          columns={[
            {
              dataIndex: 'name',
              title: menuLabels.name,
              width: 100,
              editType: 'input',
              editProps: {required: true},
            },
            {
              dataIndex: 'actionCode',
              width: 150,
              title: menuLabels.actionCode,
              editType: 'input',
              editProps: {required: true},
            },
            {
              dataIndex: 'path',
              title: menuLabels.path,
              editType: 'input',
              editProps: {required: true},
            },
          ]}
        />
      )}
    </Form>
  )
}
MenuEdit.defaultModalProps = ({model}) => {
  return {title: model?.id ? '修改菜单' : '新增菜单', width: 800}
}
export default MenuEdit
