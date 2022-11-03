import React, { useCallback, useMemo, useRef } from 'react'
import adminApi, {
  ApproveState,
  approveStateOptions,
  degreeEnumOptions,
  Enroll,
  enrollTitles,
  genderEnumOptions,
  trainingCategoryOptions,
} from '@/api/admin'
import { ApiTable, Form, FormItem, overlay } from '@/components'
import { Tabs } from 'antd'
import { TableProps } from '@/components/api-table/interface'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '@/services/user'
const { TabPane } = Tabs

const EnrollPage: PageComponent = () => {
  const navigate = useNavigate()
  const [auth] = useAuth()
  const tabProps: TableProps<Enroll> = useMemo(() => {
    return {
      titles: enrollTitles,
      tableTitle: '',

      api: adminApi.enroll.getPageList,
      columns: [
        { valueType: 'index' },
        { dataIndex: 'name', operator: 'like' },
        { dataIndex: 'phone' },
        { dataIndex: 'idCard' },
        { dataIndex: 'gender', options: genderEnumOptions },
        { dataIndex: 'degree', options: degreeEnumOptions },
        { dataIndex: 'trainingCategory', options: trainingCategoryOptions },
        { dataIndex: 'institutionName' },
        { dataIndex: 'state', options: approveStateOptions },
        {
          actions: [
            {
              onClick: async (data) => {
                navigate(`/enroll/detail?id=${data.id}&mode=approve`)
              },
              disableAutoReload: true,
              hidden: (data) => !auth.isSuperAdmin && data.state !== ApproveState.PENDING,
              title: '审核',
            },
            {
              onClick: async (data) => {
                window.open(`/enroll/detail?id=${data.id}&mode=view&hideMenu=1`)
              },
              disableAutoReload: true,
              title: '查看',
            },
            {
              onClick: async (data) => {
                if (await overlay.deleteConfirm()) {
                  return adminApi.enroll.delete(data.id)
                }
              },
              title: '删除',
              hidden: (data) => !auth.isSuperAdmin,
            },
          ],
        },
      ],
    }
  }, [auth.isSuperAdmin, navigate])
  return (
    <Tabs defaultActiveKey="2">
      <TabPane tab="全部" key="1">
        <ApiTable
          {...tabProps}
          tableTitle="报名列表"
          toolBar={[{ buttonType: 'export', title: '导出' }]}
        />
      </TabPane>
      <TabPane tab="待审核" key="2">
        <ApiTable
          {...tabProps}
          tableTitle="报名待审核列表"
          toolBar={[{ buttonType: 'export', title: '导出' }]}
          data={{ state: ApproveState.PENDING }}
        />
      </TabPane>
      <TabPane tab="已审核" key="3">
        <ApiTable
          {...tabProps}
          tableTitle="报名已审核列表"
          data={{ state: ApproveState.PASS }}
          toolBar={[{ buttonType: 'export', title: '导出' }]}
        />
      </TabPane>
      <TabPane tab="已拒绝" key="4">
        <ApiTable
          {...tabProps}
          data={{ state: ApproveState.REJECT }}
          tableTitle="报名已拒绝列表"
          toolBar={[{ buttonType: 'export', title: '导出' }]}
        />
      </TabPane>
    </Tabs>
  )
}
EnrollPage.options = { name: '报名审核', code: '000201', isMenu: true }
export default EnrollPage
