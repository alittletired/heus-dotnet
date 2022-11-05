/// <reference types="react" />
declare interface PageMenu {
  name: string
  code: string
  path: string
  children?: Menu[]
  sort?: number
}
declare interface PageAction {
  name: string
  title: string
  actionMask: long
}
declare interface PageOptions {
  //控制布局和权限 使用public将使用空布局，并不校验权限
  layout?: 'default' | 'empty' | 'public'
  menu?: PageMenu
  actions?: PageAction[]
  labels?: Record<string, string>
  parent?: PageComponent
  code?: string
}
declare interface PageComponent<P = {}> extends React.FC<P> {
  options?: PageOptions
}

declare interface ModalOptions {
  title?: string
  width?: string | number
  viewType?: ViewType
  overlayType?: OverlayType
}
declare type ModalComponentProps<P> = { model?: P }
declare interface ModalComponent<P> extends React.FC<ModalComponentProps<P>> {
  modalOptions?: (props: ModalComponentProps<P>) => ModalProps
}
