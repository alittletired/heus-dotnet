import adminApi, { LoginInput, LoginResult } from '@/api/admin'
import { selector, useRecoilState } from 'recoil'
import { withStorage } from '../utils/storage'
export interface CurrentUser extends LoginResult {
  isLogin?: boolean
}

const defaultUser = { isLogin: false } as CurrentUser
const userStorage = withStorage('auth', defaultUser)
const userState = userStorage.state

export const logout = userStorage.reset

export const login = async (token: LoginInput) => {
  var result = await adminApi.accounts.login(token)
  if (result.userId) {
    userStorage.triggerChange({ ...result, isLogin: true })
  }
}
export const getUser = userStorage.get
export const useUser = () => useRecoilState(userState)
export default userState
