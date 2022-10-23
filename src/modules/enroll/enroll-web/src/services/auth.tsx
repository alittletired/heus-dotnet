import adminApi, { AuthTokenDto } from '@/api/admin'
import { useCallback } from 'react'
import { selector, useRecoilState, useRecoilValue } from 'recoil'
import { withStorage } from './storage'

const defaultAuth: AuthToken = { isLogin: false }
const authStorage = withStorage('auth', defaultAuth)

export interface AuthToken extends AuthTokenDto {
  isLogin?: boolean
}

export const logout = authStorage.reset

export const login = (token: AuthToken) => {
  authStorage.triggerChange({ ...token, isLogin: true })
}
export const getAuth = authStorage.get
export const useAuth = () => useRecoilState(authStorage.state)
const permissionState = selector({
  key: 'permissionState',
  get: ({ get }) => {
    const auth = get(authStorage.state)
    const permissionSet = new Set<string>()
    auth.permissions?.forEach((per) => {
      permissionSet.add(per)
      permissionSet.add(per.split(':')[0])
    })
    return permissionSet
  },
})
export const usePermission = () => {
  const permissions = useRecoilValue(permissionState)
  const auth = useRecoilValue(authState)
  const hasPermission = useCallback(
    (actionCode: string) => {
      if (!actionCode) return true
      return (
        permissions.has(actionCode) || permissions.has(window.location.pathname + ':' + actionCode)
      )
    },
    [permissions],
  )
  return { hasPermission }
}
