import React, {useCallback, useState, useMemo, useRef} from 'react'
import MenuEdit from './components/MenuEdit'
import adminApi, {Resource} from '@/api/admin'
import {menuTypes, menuLabels} from './menuData'
import {Action, ApiTable, overlay} from '@/components'

const ResourcePage: PageComponent = () => {
  const menus = useRef([] as Resource[])
  const onEditMenu = useCallback(
    async (resource?: Resource) =>
      overlay.showForm(MenuEdit, resource, {menus: menus.current}),
    [],
  )
  const deleteMenu = useCallback(async (data: Resource) => {
    if (await overlay.deleteConfirm()) {
      adminApi.resources.delete(data)
    }
  }, [])

  return (
    <ApiTable
      api={adminApi.resources.getPageList}
      titles={menuLabels}
      OnApiSuccess={(data) => {
        menus.current = data.items
      }}
      pagination={{pageSize: -1}}
      tableTitle="菜单列表"
      data={{type: {$in: [1, 2]}}}
      toolBar={[
        {
          title: '新增菜单',
          buttonType: 'create',
          onClick: onEditMenu,
        },
      ]}
      useTreeTable
      columns={[
        {dataIndex: 'name'},
        {dataIndex: 'icon', valueType: 'image'},
        {dataIndex: 'path'},
        {
          dataIndex: 'type',
          render: (value) => menuTypes.find((t) => t.value === value)?.title,
        },
        {
          title: '操作',
          actions: [
            {
              onClick: onEditMenu,
              title: '编辑',
              hidden: (data) => !data.parentId,
            },
            {
              onClick: deleteMenu,
              title: '删除',
              hidden: (data) => !data.parentId,
            },
          ],
        },
      ]}
    />
  )
}
ResourcePage.options = {name: '菜单管理'}
export default ResourcePage