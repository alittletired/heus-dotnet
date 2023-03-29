import adminApi, { LoginInput, LoginResult } from '@/api/admin'
import { withGlobaState } from '@/utils/globaState'

export interface CurrentUser extends LoginResult {
  isLogin?: boolean
}
const defaultUser = { isLogin: false } as CurrentUser
const userState = withGlobaState(defaultUser, 'auth')

const user = {
  async login(token: LoginInput) {
    var result = await adminApi.accounts.login(token)
    if (result.userId) {
      userState.setState({ ...result, isLogin: true })
    }
  },
  logout() {
    userState.setState(defaultUser)
  },
  //仅用在无法使用hook的场景
  get state() {
    return userState.getState()
  },
  useState() {
    return userState.useState()
  },
}
export default user
