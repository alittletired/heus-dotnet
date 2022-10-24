import React, { useState, useRef } from 'react'
import ReactDOM from 'react-dom/client'
import { Modal, Drawer, Button } from 'antd'
import { ModalProps as AntdModalProps } from 'antd'
import './index.module.css'
export type OverlayType = 'modal' | 'drawer'
type OnDismiss = (data?: any) => void
export const OverlayContext = React.createContext({
  onDismiss(data?: any) {},
  setLoading(loading: boolean) {},
})
export interface ModalProps extends AntdModalProps {
  viewType?: ViewType
}
export interface ModalFormProps<M, P> {
  Component: ModalComponent<M, P>
  componentProps?: ModalComponentProps<M, P>
  onDismiss: OnDismiss
  onCancel: OnDismiss
  overlayType: OverlayType
}

function ModalForm<M, P>({ onCancel, onDismiss, ...props }: ModalFormProps<M, P>) {
  let { Component, componentProps, overlayType, ...rest } = props
  const [loading, setLoading] = useState(false)
  const [visible, setVisible] = useState(true)
  const onHide = () => {
    setVisible(false)
    setTimeout(onCancel, 300)
  }
  const warpClass = useRef('modal-' + new Date().getTime())
  const onOk = (e: React.MouseEvent<HTMLElement>) => {
    e.preventDefault()
    let rootDiv = document.querySelector('.' + warpClass.current)
    let form = rootDiv?.querySelector('form')
    if (!form) return

    //表单的 onsubmit 事件句柄不会被调用。
    var button = form.ownerDocument.createElement('input')
    //make sure it can't be seen/disrupts layout (even momentarily)
    button.style.display = 'none'
    //make it such that it will invoke submit if clicked
    button.type = 'submit'
    //append it and click it
    form?.appendChild(button).click()
    //if it was prevented, make sure we don't get a build up of buttons
    form?.removeChild(button)

    return false
  }

  const [modalProps, setModalProps] = useState<ModalProps>(() => {
    if (Component.defaultModalProps) {
      return Component.defaultModalProps(componentProps!)
    }
    return {}
  })

  // const setModalProps = useCallback((mprops: FormModalProps) => {
  //   setTimeout(() => setProps(mprops), 16)
  // }, [])
  const footer = modalProps.viewType === 'view' ? false : undefined
  const children = (
    <OverlayContext.Provider value={{ setLoading, onDismiss }}>
      <Component
        {...componentProps}
        setModalProps={(nextProps) => setModalProps((prev) => ({ ...prev, ...nextProps }))}
      />
    </OverlayContext.Provider>
  )
  if (overlayType === 'modal')
    return (
      <Modal
        maskClosable={false}
        wrapClassName={warpClass.current}
        visible={visible}
        confirmLoading={loading}
        className="modal"
        onOk={onOk}
        footer={footer}
        {...modalProps}
        onCancel={onHide}>
        {children}
      </Modal>
    )
  return (
    <Drawer
      destroyOnClose
      onClose={onHide}
      visible={visible}
      width={modalProps.width ?? 800}
      className={warpClass.current}
      footer={
        <div
          style={{
            textAlign: 'right',
          }}>
          <Button onClick={onCancel} style={{ marginRight: 8 }}>
            取消
          </Button>
          <Button onClick={onOk} type="primary">
            确认
          </Button>
        </div>
      }
      {...modalProps}>
      {children}
    </Drawer>
  )
}
export function showDrawer<M, P = any>(
  Component: ModalComponent<M, P>,
  model: M,
  props?: P,
): Promise<any> {
  return showForm(Component, model, props, 'drawer')
}
export function showForm<M, P = any>(
  Component: ModalComponent<M, P>,
  model: M,
  props?: P,
  overlayType: OverlayType = 'modal',
): Promise<any> {
  return new Promise((resolve, reject) => {
    let div = document.createElement('div')
    div.id = 'div_' + Math.random()
    document.body.appendChild(div)
    const root = ReactDOM.createRoot(div)
    let removeNode = () => {
      requestAnimationFrame(() => {
        root.unmount()
        // console.warn('onDismiss', div, div.parentNode)
        div.parentNode?.removeChild(div)
      })
    }
    let onDismiss = (data?: any) => {
      resolve(data)
      removeNode()
    }
    let onCancel = (data?: any) => {
      reject(data)
      removeNode()
    }
    // console.warn('createPortal')

    root.render(
      <ModalForm<M, P>
        Component={Component}
        onDismiss={onDismiss}
        onCancel={onCancel}
        componentProps={{ model, ...props }}
        overlayType={overlayType}
      />,
    )
  })
}

export function confirm(content: string): Promise<boolean> {
  return new Promise((resolve, reject) => {
    return Modal.confirm({
      content,
      onOk: async () => {
        resolve(true)
      },
      onCancel: () => reject(),
    })
  })
}
export function deleteConfirm<T>(content: string = '确定删除该记录吗?'): Promise<boolean> {
  return confirm(content)
}
export default {
  showForm,
  showDrawer,
  deleteConfirm,
  confirm,
}
