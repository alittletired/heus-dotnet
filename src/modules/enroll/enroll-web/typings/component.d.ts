/// <reference types="react" />

declare interface PageOptions {
  path?: string
  //控制布局和权限 使用public将使用空布局，并不校验权限
  layout?: 'default' | 'empty' | 'public'
  name?: string

  parent?: PageComponent
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

declare interface ModalComponent<P> extends React.FC<{ model?: P }> {
  modalOptions?: (props: { model?: P }) => ModalProps
}
