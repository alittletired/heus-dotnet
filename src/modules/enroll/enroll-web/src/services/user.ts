import adminApi, { LoginInput, LoginResult } from '@/api/admin'

import { withGlobaState } from '../utils/globaState'
export interface CurrentUser extends LoginResult {
  isLogin?: boolean
}

const defaultUser = { isLogin: false } as CurrentUser
const userStorage = withGlobaState(defaultUser, 'auth')
export const logout = userStorage.resetState
export const login = async (token: LoginInput) => {
  var result = await adminApi.accounts.login(token)
  if (result.userId) {
    userStorage.setState({ ...result, isLogin: true })
  }
}
export const getUser = userStorage.getState
export const useUser = () => userStorage.useState
export default userStorage.state
