import React from 'react'
export interface PageContext {
  doLoading: (func: () => Promise<any>) => Promise<void>
  loading: boolean
  labels?: Record<string, string>
}
const PageContext = React.createContext({} as PageContext)
export const usePageContext = () => React.useContext(PageContext)
export default PageContext
