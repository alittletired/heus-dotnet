import appConfig from '@/config/siteSettings'
import { atom, useAtom } from 'jotai'
const appConfigState = atom({ ...appConfig, collapsed: false })

export default appConfigState
export const useAppConfig = () => useAtom(appConfigState)
