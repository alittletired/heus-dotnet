import adminApi, {
  ApproveState,
  degreeEnumOptions,
  Enroll,
  EnrollApproveDto,
  enrollTitles,
  genderEnumOptions,
  institutionRecordOptions,
  paymentStateOptions,
  trainingCategoryOptions,
} from '@/api/admin'
import { ApiTable, Form, FormItem, Loading, overlay } from '@/components'
import { useAuth } from '@/services/auth'
import { useQuery } from '@/utils/routerUtils'
import { Button, Card, Col, Image, message, Row, Space, Tooltip } from 'antd'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
const imageProps = { width: 80, height: 80 }
const EnrollDetailPage: PageComponent = () => {
  const query = useQuery()
  const navigate = useNavigate()

  const enrollId = Number(query.get('id'))
  const mode = query.get('mode')
  const [enroll, setEnroll] = useState({} as Enroll)
  const loadData = async () => {
    let enroll = await adminApi.enroll.getById(enrollId)
    setEnroll(enroll)
  }
  const canEdit = mode !== 'view' && enroll.state === ApproveState.PENDING
  return (
    <Loading loadData={loadData}>
      <Form disabled titles={enrollTitles} initialValues={enroll}>
        <Row>
          <Col span={12}>
            <FormItem.Input name="name" />
            <FormItem.Select name="gender" options={genderEnumOptions} />
            <FormItem.Input name="phone" />
            <FormItem.Select name="degree" options={degreeEnumOptions} />
            <FormItem.Input name="idCard" />
            <FormItem.Input name="institutionName" />

            <FormItem.Item label="身份证照片">
              <Image {...imageProps} src={enroll.idCardFace} />
              <Image {...imageProps} style={{ marginLeft: 24 }} src={enroll.idCardNational} />
            </FormItem.Item>
            <FormItem.Item label="登记照">
              <Image {...imageProps} src={enroll.registrationPhoto} />
            </FormItem.Item>
            <FormItem.Input name="practiceDate" />
          </Col>
          <Col span={12}>
            <FormItem.Select name="trainingCategory" options={trainingCategoryOptions} />
            <FormItem.Input name="institutionName" />
            <FormItem.Input name="institutionPhone" />
            <FormItem.Input name="institutionAddress" />
            <FormItem.Select
              name="institutionRecord"
              placeholder=""
              disabled
              options={institutionRecordOptions}
            />
            <FormItem.Input name="belongsArea" />
            <FormItem.Item label="学历照">
              <Image {...imageProps} src={enroll.degreePhoto} />
            </FormItem.Item>
          </Col>
        </Row>
      </Form>
      <Form
        titles={enrollTitles}
        initialValues={enroll}
        onSuccess={(enroll) => {
          setEnroll(enroll)
          message.success('操作成功')
        }}
        onBefore={(data) => ({ ...data, state: data['state1'] })}
        api={adminApi.enroll.approve}>
        <Card title="审核">
          <Form.Item label="支付状态">
            <Space>
              <FormItem.RadioGroup
                disabled={!canEdit}
                name="payment"
                noStyle
                options={paymentStateOptions}
              />
              <Tooltip className="ant-form-item-extra" style={{ fontSize: '12px' }}>
                （确认支付报名费用后打勾）
              </Tooltip>
            </Space>
          </Form.Item>

          <FormItem.RadioGroup
            required
            disabled={!canEdit}
            name="state1"
            label="是否受理"
            options={[
              { value: ApproveState.PASS, label: '受理' },
              { value: ApproveState.REJECT, label: '拒绝受理' },
            ]}
          />
          <FormItem.TextArea
            disabled={!canEdit}
            rows={5}
            name="message"
            placeholder="请输入拒绝理由"
          />

          <FormItem.Item label=" " colon={false}>
            <Space>
              {!query.has('hideMenu') && <Button onClick={() => navigate(-1)}>返回</Button>}

              {canEdit && (
                <Button type="primary" htmlType="submit">
                  提交
                </Button>
              )}
            </Space>
          </FormItem.Item>
        </Card>
      </Form>
    </Loading>
  )
}
export default EnrollDetailPage
