import React, { useState, useRef, PropsWithChildren, ReactNode } from 'react'
import { Modal, Drawer, Button } from 'antd'
export type OverlayType = 'modal' | 'drawer'

type OnDismiss = (data?: any) => void
interface OverlayContext {
  onDismiss: OnDismiss
  setLoading: (loading: boolean) => any
  setModalProps: React.Dispatch<React.SetStateAction<ModalProps>>
}
let openModal = null as unknown as React.Dispatch<React.SetStateAction<ReactNode>>
export const OverlayContainer: React.FC<PropsWithChildren> = (props) => {
  const [modal, setModal] = useState<ReactNode>(undefined)
  openModal = setModal
  return <div id="modal-root">{modal}</div>
}
export const OverlayContext = React.createContext({
  onDismiss(data?: any) {},
  setLoading(loading: boolean) {},
} as OverlayContext)

export interface ModalFormProps<M, P> {
  Component: ModalComponent<M, P>
  componentProps: ModalComponentProps<M, P>
  onDismiss: OnDismiss
  onCancel: OnDismiss
  overlayType: OverlayType
}

function ModalForm<M, P>({ onCancel, onDismiss, ...props }: ModalFormProps<M, P>) {
  let { Component, componentProps, overlayType } = props
  const [loading, setLoading] = useState(false)
  const [open, setOpen] = useState(true)
  const onHide = () => {
    setOpen(false)
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

  const [modalProps, setModalProps] = useState<ModalProps>(
    Component.modalProps?.(componentProps) ?? {},
  )

  // const setModalProps = useCallback((mprops: FormModalProps) => {
  //   setTimeout(() => setProps(mprops), 16)
  // }, [])
  const footer = modalProps.viewType === 'view' ? false : undefined
  const children = (
    <OverlayContext.Provider value={{ setLoading, onDismiss, setModalProps }}>
      <Component {...componentProps} />
    </OverlayContext.Provider>
  )
  if (overlayType === 'modal')
    return (
      <Modal
        maskClosable={false}
        wrapClassName={warpClass.current}
        open={open}
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
      open={open}
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
export function openDrawer<M, P = any>(
  Component: ModalComponent<M, P>,
  props: ModalComponentProps<M, P>,
): Promise<any> {
  return openForm(Component, props, 'drawer')
}
export function openForm<M, P = any>(
  Component: ModalComponent<M, P>,
  props: ModalComponentProps<M, P>,
  overlayType: OverlayType = 'modal',
): Promise<any> {
  return new Promise((resolve, reject) => {
    let onDismiss = (data?: any) => {
      resolve(data)
      openModal(null)
    }
    let onCancel = (data?: any) => {
      reject(data)
      openModal(null)
    }

    // console.warn('createPortal')
    var modalDom = (
      <ModalForm<M, P>
        Component={Component}
        onDismiss={onDismiss}
        onCancel={onCancel}
        componentProps={props}
        overlayType={overlayType}
      />
    )
    openModal(modalDom)
  })
}

export function confirm(content: string): Promise<boolean> {
  return new Promise((resolve) => {
    return Modal.confirm({
      content,
      onOk: () => resolve(true),
      onCancel: () => resolve(false),
    })
  })
}
export function deleteConfirm<T>(content: string = '确定删除该记录吗?'): Promise<boolean> {
  return confirm(content)
}
const overlay = {
  openForm,
  openDrawer,
  deleteConfirm,
  confirm,
}
export default overlay
