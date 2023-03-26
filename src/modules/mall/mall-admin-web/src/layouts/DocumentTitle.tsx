import { findMenusByPath } from '@/services/menu'
import useRouter from '@/services/router'
import { useEffect } from 'react'
import { useAppConfig } from './appConfig'

const DocumentTitle = () => {
  const router = useRouter()
  const [appConfig] = useAppConfig()
  useEffect(() => {
    const pathname = location.pathname
    let menus = findMenusByPath(pathname)
    document.title = menus?.[menus.length - 1]?.name || appConfig.siteName
  }, [router.pathname, appConfig.siteName])

  return <></>
}
export default DocumentTitle
