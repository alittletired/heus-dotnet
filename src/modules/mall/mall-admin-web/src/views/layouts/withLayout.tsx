import { ComponentType, FC } from 'react'
import ProtectedLayout from './ProtectedLayout'

function withLayout<P>(Component: FC<P>, layout: Layout = 'empty') {
  if (layout === 'empty') {
    return Component
  }
  if (Component.displayName == 'ErrorPage') {
    return Component
  }

  const LayoutComponent: FC<P> = (props) => {
    return (
      <ProtectedLayout>
        <Component {...props} />
      </ProtectedLayout>
    )
  }
  return LayoutComponent
}
export default withLayout
