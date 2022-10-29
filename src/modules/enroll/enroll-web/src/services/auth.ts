import { PropsWithChildren, useCallback } from 'react'
import { selector, useRecoilValue, useRecoilValueLoadable } from 'recoil'
import userState from './user'
import adminApi from '@/api/admin'

export class AuthState {
  constructor(actionCodes: string[]) {
    this.state = new Set<string>(actionCodes)
  }
  public state: Set<string>
  isAllow = (actionCode?: string) => {
    if (!actionCode) return true
    return this.state.has(actionCode) || this.state.has(window.location.pathname + ':' + actionCode)
  }
}

const authState = selector({
  key: 'authState',
  get: async ({ get }) => {
    const user = get(userState)
    if (!user.isLogin) return new AuthState([])

    let resourceCodes = await adminApi.users.getResourceCodes(user.userId)
    return new AuthState(resourceCodes)
  },
})
export default authState
export const useAtuh = () => useRecoilValue(authState)
