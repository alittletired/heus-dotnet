import adminApi, { LoginInput, LoginResult } from '@/api/admin'
import { withGlobaState } from '@/utils/globaState'

export interface CurrentUser extends LoginResult {
  isLogin?: boolean
}
const defaultUser = { isLogin: false } as CurrentUser
const userState = withGlobaState(defaultUser, 'auth')

export const logout = () => userState.setState(defaultUser)
export const login = async (token: LoginInput) => {
  var result = await adminApi.accounts.login(token)
  if (result.userId) {
    userState.setState({ ...result, isLogin: true })
  }
}
export const getUser = userState.getState
export const useUser = userState.useState
export default userState
