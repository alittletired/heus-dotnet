import { Locale } from '@/config/locales/Locale'
import { useMemo } from 'react'
import { useIntl } from 'react-intl'
type LocaleKeys = keyof Locale
export const useLocale = () => {
  const init = useIntl()
  const obj = useMemo(() => {
    const formatMessage = (id: LocaleKeys) => {
      return init.formatMessage({ id })
    }
    return { formatMessage }
  }, [init])
  return obj
}
