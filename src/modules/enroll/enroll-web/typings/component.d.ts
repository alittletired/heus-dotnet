/// <reference types="react" />
declare interface PageOptions {
  path?: string
  //控制布局和权限 使用public将使用空布局，并不校验权限
  layout?: 'default' | 'empty' | 'public'
  name: string

  parent?: PageComponent
}
interface PageComponent<P = {}> extends React.FC<P> {
  options?: PageOptions
}
