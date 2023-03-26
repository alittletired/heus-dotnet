import userState from './user'
import menus from '@/config/menus'
import adminApi, { UserActionRight, ResourceDto, ActionDto } from '@/api/admin'
import useRouter from './router'
import { useCallback } from 'react'
import { atom, useAtomValue } from 'jotai'

const actionMap: Map<string, ActionDto[]> = new Map()
function initActionMap(menuArr: ResourceDto[]) {
  for (let menu of menuArr) {
    let { children, actions = [] } = menu
    if (children) {
      initActionMap(children)
      continue
    }
    actionMap.set(menu.path, actions)
  }
}
initActionMap(menus)
export function findAction(path: string, actionName?: string) {
  if (!actionName) return null
  const actions = actionMap.get(path)
  if (!actions) return null
  return actions.find((s) => s.name === actionName)
}

const authState = atom(async (get) => {
  const user = get(userState)
  let authMap: Map<string, number> = new Map()
  if (!user.isLogin) {
    return authMap
  }
  let authState = await adminApi.resources.getUserActionRights(user.userId)
  authState.forEach((a) => authMap.set(a.resourcePath, a.flag))
  return authMap
})

export const useAuth = () => {
  const router = useRouter()
  const state = useAtomValue(authState)
  const hasActionRight = useCallback(
    (path: string, actionName?: string) => {
      if (!actionName) return true
      const action = findAction(path, actionName)
      if (!action) return true
      const flag = state.get(path)
      return !!flag && (flag & action.flag) === action.flag
    },
    [state],
  )

  const hasRight = useCallback(
    (actionName?: string) => {
      return hasActionRight(router.asPath, actionName)
    },
    [router.asPath, hasActionRight],
  )

  return { state, hasRight, hasActionRight }
}
export default authState
