import { Locale } from '@/config/locales/Locale'
import { useRef } from 'react'
import { useIntl } from 'react-intl'
import { withGlobaState } from './globaState'
import globalConfigState from '@/services/globalConfig'
import dayjs from 'dayjs'
const locales = {
  'zh-CN': import('../config/locales/zh-CN'),
  'en-US': import('../config/locales/en-US'),
}

//, 'de-DE': 'de'
type SupportLocales = keyof typeof locales
const dayjsMappings: Record<SupportLocales, string> = { 'zh-CN': 'zh-cn', 'en-US': 'en' }
const localeState = withGlobaState(
  globalConfigState.getState().defaultLocale as SupportLocales,
  'locale',
)
localeState.sub((locale) => {
  dayjs.locale(dayjsMappings[locale])
})

type LocaleKeys = keyof Locale
export const useLocale = () => {
  const init = useIntl()
  const local = useRef({
    formatMessage: (id: LocaleKeys) => {
      return init.formatMessage({ id })
    },
  })
  return local.current
}

export default localeState
