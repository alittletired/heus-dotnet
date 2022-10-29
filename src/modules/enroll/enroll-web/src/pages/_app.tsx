import '../styles/globals.css'
import '../styles/layout.css'
import type { AppProps } from 'next/app'
import { AppContainer } from '@/layouts'
function MyApp(props: AppProps) {
  return <AppContainer {...props} />
}

export default MyApp
