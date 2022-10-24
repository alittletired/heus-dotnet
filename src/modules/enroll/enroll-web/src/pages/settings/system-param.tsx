import React, {useCallback, useState} from 'react'
import adminApi, {SysParam} from '@/api/admin'
import {Form, FormItem, Loading} from '@/components'
import {Button, Card} from 'antd'
const titles = {maxDueBean: '最大欠费豆子', maxChallengeNum: '每日擂台挑战次数'}

const SysParamPage: PageComponent = () => {
  const [param, setParam] = useState<SysParam>({})
  const loadData = useCallback(async () => {
    let param = await adminApi.sysParams.get()
    // console.warn(param)

    setParam(param)
  }, [])
  return (
    <Loading loadData={loadData}>
      <Card title="系统参数">
        <Form titles={titles} initialValues={param} api={adminApi.sysParams.save}>
          {/* <FormItem.InputNumber name="maxDueBean" min={0} required />
          <FormItem.InputNumber name="maxChallengeNum" min={0} required /> */}
          <Form.Item label=" " colon={false}>
            <Button type="primary" htmlType="submit">
              保存
            </Button>
          </Form.Item>
        </Form>
      </Card>
    </Loading>
  )
}
SysParamPage.options = {name: '系统参数', path: '/auth/system-param'}
export default SysParamPage
