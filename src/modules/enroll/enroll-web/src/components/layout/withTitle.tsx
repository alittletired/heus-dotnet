import React, {useEffect} from 'react'
import {getMenuByPath} from '@/services/menu'
import {useLocation} from 'react-router-dom'
export default function withTitle<T>(Component: PageComponent<T>) {
  const WithTitleComponent: PageComponent = (props) => {
    const location = useLocation()
    useEffect(() => {
      const pathname = location.pathname
      let currMenu = getMenuByPath(pathname)
      document.title = currMenu
        ? currMenu.name
        : (Component as PageComponent).options?.name
    }, [location.pathname])
    return <Component {...props} />
  }
  WithTitleComponent.options = Component.options
  return WithTitleComponent
}
