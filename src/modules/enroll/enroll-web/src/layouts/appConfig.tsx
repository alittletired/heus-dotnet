import appConfig from '@/config/appConfig'
import { atom, useRecoilState, useRecoilValue } from 'recoil'
const appConfigState = atom({
  key: 'appConfig',
  default: { ...appConfig, collapsed: false },
})

export default appConfigState
export const useAppConfig = () => useRecoilState(appConfigState)
