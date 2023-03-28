import userState, { useUser } from './user'
import menus from '@/config/menus'
import adminApi, { UserActionRight, ResourceDto, ActionDto } from '@/api/admin'
import useRouter from './router'
import { PropsWithChildren, useCallback, useContext, useEffect, useMemo, useState } from 'react'
import React from 'react'
const AuthContext=React.createContext(new Map<string, number>());

export const AuthProvider:React.FC<PropsWithChildren>=(props)=>{
const [loaded,setLoaded]=useState(false)
const [user] = useUser()
const [authMap,setAuthMap]=useState(new Map<string, number>())
useEffect(()=>{
if(!user.userId)
{setLoaded(true)
return}
adminApi.resources.getUserActionRights(user.userId).then(userActionRights=>{
  const authMap =new Map<string, number>()
  userActionRights.forEach((a) => authMap.set(a.resourcePath, a.flag))
  setAuthMap(authMap)
  setLoaded(true)
})
 

},[user.userId]) 
return <AuthContext.Provider value={authMap}>
  {loaded&& props.children}
</AuthContext.Provider>
}

export const useAuth = () => {
  const router = useRouter()
  const authMap=useContext(AuthContext)
  const auth=useMemo(()=>{
const   hasRight=(path: string, actionName?: string)=>{

  
}

export default authState
