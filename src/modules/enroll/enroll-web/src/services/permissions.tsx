import { useCallback } from 'react'
import { selector, useRecoilValue } from 'recoil'
import userState from './user'
import adminApi from '@/api/admin'
const permissionState = selector({
  key: 'permissionState',
  get: async ({ get }) => {
    const user = get(userState)

    if (!user.isLogin) return new Set<string>()
    let resourceCodes = await adminApi.users.getResourceCodes(user.userId)
    return new Set<string>(resourceCodes)
  },
})
export const usePermission = () => {
  const permissions = useRecoilValue(permissionState)

  const hasPermission = useCallback(
    (actionCode?: string) => {
      if (!actionCode) return true
      return (
        permissions.has(actionCode) || permissions.has(window.location.pathname + ':' + actionCode)
      )
    },
    [permissions],
  )
  return { hasPermission }
}
export const LoadPermission = () => {
  var permission = useRecoilValue(permissionState)
  return <></>
}
