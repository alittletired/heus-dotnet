import { ComponentType } from 'react'
import ProtectedLayout from './ProtectedLayout'
function withLayout<P>(Component: ComponentType<P>) {
  var layout = (Component as PageComponent).options?.layout
  if (layout === 'empty') {
    return Component
  }
  const LayoutComponent: React.FC<P> = (props) => {
    return (
      <ProtectedLayout>
        <Component {...props} />
      </ProtectedLayout>
    )
  }
  return LayoutComponent
}
export default withLayout
