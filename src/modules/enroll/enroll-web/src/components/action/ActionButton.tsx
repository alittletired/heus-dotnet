import React from 'react'
import icons from '../icons'
import { Button, message } from 'antd'

import overlay from '../overlay'
import { isApiError } from '@/services/http'
import { ButtonProps as AntdButtonProps, ButtonType } from 'antd'
type GetTitle<T> = (data: T) => string
export interface ActionButtonProps<T> {
  code?: string
  component?: ModalComponent<T>
  onClick?: (data?: T) => Promise<any>
  data?: T
  title?: string | GetTitle<T>
  icon?: keyof typeof icons
  onSuccess?: (res: any) => any
  onError?: (e: any) => any
  disabled?: boolean | FuncSync<T, boolean>
  hidden?: boolean | FuncSync<T, boolean>
  type?: ButtonType
}

export default function ActionButton<T>(
  props: ActionButtonProps<T> &
    Omit<AntdButtonProps, 'onClick' | 'icon' | 'disabled' | 'hidden' | 'title'>,
) {
  let {
    code,
    icon,
    type,
    title,
    data,
    component,
    hidden,
    disabled,
    children,
    onSuccess,
    onError,

    ...rest
  } = props
  const IconComponent = icons[icon]
  const iconDom = IconComponent ? <IconComponent /> : undefined
  code = code ?? icon
  let handleClick = async () => {
    try {
      //   let res: any = await props.onClick?.(data)
      let res
      if (component) {
        let showFn = overlay.showForm
        if (component.defaultModalProps?.(data).overlayType === 'drawer') {
          showFn = overlay.showDrawer
        }
        res = await showFn(component, data)
      } else {
        res = await props.onClick(data)
      }
      if (onSuccess) {
        onSuccess(res)
      } else {
        message.success('操作成功')
      }
    } catch (ex) {
      if (onError) {
        onError(ex)
      } else {
        isApiError(ex) && message.error(ex.data.msg)
        console.warn('onunhandledrejection', ex)
      }
    }
  }
  let buttonDisabled = undefined
  if (typeof hidden === 'function') {
    if (hidden(data)) return null
  } else if (hidden === true) {
    return null
  }
  if (typeof disabled === 'function') {
    buttonDisabled = disabled(data)
  } else if (typeof disabled === 'boolean') {
    buttonDisabled = disabled
  }
  if (!children) {
    if (typeof title === 'function') {
      children = title(data)
    } else {
      children = title
    }
  }
  return (
    <Button {...rest} onClick={handleClick} icon={iconDom} type={type} disabled={buttonDisabled}>
      {children}
    </Button>
  )
}
