import '../styles/globals.css'
import type { AppProps } from 'next/app'
import { AppContainer } from '@/components/layout'
import siteConfig from '@/config/siteConfig'
import menus from '@/config/menus'
function MyApp({ Component, pageProps }: AppProps) {
  return (
    <AppContainer {...siteConfig} menus={menus}>
      <Component {...pageProps} />
    </AppContainer>
  )
}

export default MyApp
