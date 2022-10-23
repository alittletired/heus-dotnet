import adminApi, { AuthTokenDto } from '@/api/admin'
import { useCallback } from 'react'

import { atom, AtomEffect, selector, useRecoilState, useRecoilValue } from 'recoil'
class MyStorage<T> {
  private _key: string
  private _defaultValue: T
  private _value: T
  public constructor(key: string, defaultValue: T) {
    this._key = key
    this._defaultValue = defaultValue
    const value = localStorage.getItem(this._key)
    if (value) {
      this._value = JSON.parse(value)
    } else {
      this._value = this._defaultValue
    }
  }
  private _handler: (value: T) => void
  public onChange = (handler: (value: T) => void) => {
    this._handler = handler
  }
  public set = (newValue: T) => {
    this._value = newValue
    localStorage.setItem(this._key, JSON.stringify(newValue))
  }
  public triggerChange = (value: T) => {
    this.set(value)
    this._handler?.(value)
  }
  public get = () => {
    return this._value
  }
  public reset = () => {
    this._value = this._defaultValue
    localStorage.removeItem(this._key)
  }
}
function withStorage<T>(key: string, defaultValue: T) {
  const myStorage = new MyStorage<T>(key, defaultValue)
  const effect: AtomEffect<T> = ({ setSelf, trigger, onSet }) => {
    if (trigger === 'get') {
      setSelf(myStorage.get())
    }
    myStorage.onChange((value) => setSelf(value))
    onSet((newValue, _, isReset) => {
      isReset ? myStorage.reset() : myStorage.set(newValue)
    })
    return () => {
      myStorage.onChange(null)
    }
  }
  return { ...myStorage, effect }
}

const defaultAuth: AuthToken = { isLogin: false }
const { get, triggerChange, effect } = withStorage('auth', defaultAuth)
const authState = atom({
  key: 'auth',
  effects: [effect],
})

export interface AuthToken extends AuthTokenDto {
  isLogin?: boolean
}

export const logout = () => triggerChange(defaultAuth)

export const login = (token: AuthToken) => {
  triggerChange({ ...token, isLogin: true })
}
export const getAuth = get
export const useAuth = () => useRecoilState(authState)
const permissionState = selector({
  key: 'permissionState',
  get: ({ get }) => {
    const auth = get(authState)
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
