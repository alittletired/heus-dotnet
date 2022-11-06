import { selector, useRecoilValue } from 'recoil'
import userState from './user'
import adminApi, { ActionRightDto, ResourceDto } from '@/api/admin'

export class AuthState {
  constructor(public actionRight: ActionRightDto[]) {}
  hasAuthority = (path: string, actionName?: string) => {
    if (!actionName) return true
    return true
  }
}

const authState = selector({
  key: 'authState',
  get: async ({ get }) => {
    const user = get(userState)
    if (!user.isLogin) return new AuthState([])

    let resourceCodes = await adminApi.resources.getUserActionRights(user.userId)
    return new AuthState(resourceCodes)
  },
})
export default authState
export const useAtuh = () => useRecoilValue(authState)
