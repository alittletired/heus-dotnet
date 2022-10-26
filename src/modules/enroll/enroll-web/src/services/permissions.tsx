const permissionState = selector({
    key: 'permissionState',
    get: ({ get }) => {
      const auth = get(userStorage.state)
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