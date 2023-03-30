import adminApi, { User } from '@/api/admin'
import { Form, FormItem } from '@/components'

const ResetPassword: ModalComponent<User> = (props) => {
  let { id: userId, account } = props.model
  return (
    <Form initialValues={{ userId, account }} api={adminApi.users.resetPassword}>
      <FormItem.Input name="account" readOnly />
      <FormItem.Input name="newPassword" required />
    </Form>
  )
}
ResetPassword.defaultModalProps = () => ({ title: '重置密码' })
export default ResetPassword
