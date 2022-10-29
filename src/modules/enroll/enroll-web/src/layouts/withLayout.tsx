import { ComponentType } from 'react'
import ProtectedLayout from './ProtectedLayout'

function withLayout<P>(Component: ComponentType<P>) {
  var layout = (Component as PageComponent).options?.layout
  if (layout === 'empty') {
    return Component
  }
  if (Component.displayName == 'ErrorPage') {
    return Component
  }
  console.warn('layout', layout, Component)

  const LayoutComponent: ComponentType<P> = (props) => {
    return (
      <ProtectedLayout>
        <Component {...(props as any)} />
      </ProtectedLayout>
    )
  }
  return LayoutComponent
}
export default withLayout
