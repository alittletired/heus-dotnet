import globalConfig from '@/config/globalConfig'
import { withGlobaState } from '@/utils/globaState'
const globalConfigState = withGlobaState({ ...globalConfig, collapsed: false })

export default globalConfigState
export const useGlobalConfig = globalConfigState.useState
