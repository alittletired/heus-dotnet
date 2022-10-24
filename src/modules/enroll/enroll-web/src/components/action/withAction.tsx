import {ComponentType} from 'react'
import {useAuth, usePermission} from '@/services/auth'
export interface Action {
  code: string
  menuPath?: string
}

export default function withAction<P>(OriginComponent: React.FC<P>) {
  const ActionComponent: React.FC<P & Action> = (props) => {
    const {hasPermission} = usePermission()
    let {menuPath = window.location.pathname, code, ...rest} = props
    if (!hasPermission(`${menuPath}/${code}`)) return null
    return <OriginComponent {...(rest as any)} />
  }
  return ActionComponent
}
