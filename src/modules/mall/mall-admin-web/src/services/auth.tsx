import user from './user'
import menus from '@/config/menus'
import adminApi, { UserActionRight, ResourceDto, ActionDto } from '@/api/admin'
import useRouter from './router'
import { PropsWithChildren, useCallback, useContext, useEffect, useMemo, useState } from 'react'
import React from 'react'
const AuthContext = React.createContext(new Map<string, number>())

export const AuthProvider: React.FC<PropsWithChildren> = (props) => {
  const [loaded, setLoaded] = useState(false)
  const [userState] = user.useState()
  const [authMap, setAuthMap] = useState(new Map<string, number>())
  useEffect(() => {
    if (!userState.userId) {
      setLoaded(true)
      return
    }
    adminApi.resources.getUserActionRights(userState.userId).then((userActionRights) => {
      const authMap = new Map<string, number>()
      userActionRights.forEach((a) => authMap.set(a.resourcePath, a.flag))
      setAuthMap(authMap)
      setLoaded(true)
    })
  }, [userState.userId])
  return <AuthContext.Provider value={authMap}>{loaded && props.children}</AuthContext.Provider>
}

export const useAuth = () => {
  const authMap = useContext(AuthContext)
  const auth = useMemo(() => {
    function hasRight(funcCode: string, actionCode: number) {
      const actionFlag = authMap.get(funcCode)
      if (!actionFlag) return false
      return (actionFlag & actionCode) === actionCode
    }

    return { hasRight }
  }, [authMap])
  return auth
}
