/// <reference types="react" />
declare interface PageMenu {
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
declare type Layout = 'default' | 'empty' | 'public'
declare interface PageOptions {
  //控制布局和权限 使用public将使用空布局，并不校验权限
  layout?: Layout
  actions?: PageAction[]
  parent?: PageComponent
  code?: string
  name?: string
}
declare type PageComponent<P> = React.FC<P> & {
  options?: PageOptions
}

declare interface ModalProps {
  title?: string
  width?: string | number
  viewType?: ViewType
  overlayType?: OverlayType
}
declare type ModalComponentProps<M, P = {}> = { model: M } & P
declare interface ModalComponent<M, P = {}> extends React.FC<{ model: M } & P> {
  modalProps?: (props: ModalComponentProps<M, P>) => ModalProps
}
type Dispatch<A> = (value: A) => void
