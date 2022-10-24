import {useCallback} from 'react'
import {overlay} from '..'
import icons, {IconKey} from '../icons'
export interface ActionIconProps<T> {
  icon: IconKey
  title: string
  data?: T
  onSuccess?: Function
  onClick?: (data: T) => any
  component?: ModalComponent<T>
}

export default function ActionIcon<T>(props: ActionIconProps<T>) {
  let {icon, component, data, onSuccess, onClick, ...rest} = props
  let IconComponent = icons[icon]
  const handleClick = useCallback(
    async (e: React.MouseEvent) => {
      e.stopPropagation()
      e.preventDefault()
      console.warn('props', props)

      let res
      if (component) {
        let showFn = overlay.showForm
        if (component.defaultModalProps?.(data).overlayType === 'drawer') {
          showFn = overlay.showDrawer
        }
        res = await showFn(component, data)
      } else {
        res = await onClick(data)
      }
      onSuccess?.()
    },
    [component, data, onSuccess, onClick, props],
  )
  return <IconComponent key={rest.title} {...rest} onClick={handleClick} />
}
