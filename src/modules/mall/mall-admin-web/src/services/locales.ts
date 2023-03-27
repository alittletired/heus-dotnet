import { Locale } from '@/config/locales/Locale'
import { useRef } from 'react'
import { useIntl } from 'react-intl'
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
