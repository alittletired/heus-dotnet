import { selector, useRecoilValue } from 'recoil'
import userState from './user'
import menus from '@/config/menus'
import adminApi, { UserActionRight, ResourceDto, ActionDto } from '@/api/admin'
import useRouter from './router'
import { useCallback } from 'react'

const actionRightMap: Map<string, ActionDto[]> = new Map()
function initActionRight(menuArr: ResourceDto[]) {
  for (let menu of menuArr) {
    let { children, actions = [] } = menu
    if (children) {
      initActionRight(children)
      continue
    }
    actionRightMap.set(menu.path, actions)
  }
}
initActionRight(menus)
export function findAction(path: string, actionName?: string) {
  if (!actionName) return null
  const actions = actionRightMap.get(path)
  if (!actions) return null
  return actions.find((s) => s.name === actionName)
}

const userAuthState = selector({
  key: 'userAuthState',
  get: async ({ get }) => {
    const user = get(userState)
    if (!user.isLogin) return new Map<string, number>()
    let authState = await adminApi.resources.getUserActionRights(user.userId)
    const authMap = new Map(authState.map((a) => [a.resourcePath, a.flag]))
    return authMap
  },
})

export const useAuth = () => {
  const state = useRecoilValue(userAuthState)
  const router = useRouter()
  const hasPageRight = useCallback(
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
    (actionName: string) => hasPageRight(router.asPath, actionName),
    [router.asPath, hasPageRight],
  )

  return { state, hasRight }
}
export default userAuthState
