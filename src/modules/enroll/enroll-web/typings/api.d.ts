interface PageRequest {
  pageIndex?: number
  pageSize?: number
  orderBy?: string
}
type DynamicQuery<T> = {
  [P in keyof T]?: T[P] | { ['$' + key in Operator]?: T[P] | Array<T[P]> }
} & PageRequest
type PageList<T> = { items: T[]; total: number }
type Api<D = any, R = any> = (data: D) => Promise<R>
type ParamApi<D = any, P = any, R = any> = (params: P, data: D) => Promise<R>
interface ApiProps<D = any, P = any, R = any> {
  api?: Api<D, R> | ParamApi<D, P, R>
  data?: D
  onBefore?: (data: D) => Promise<D | boolean> | D | boolean
  onSuccess?: (data: R) => any
  onFail?(err: any): void
}

type Operator =
  | 'eq'
  | 'neq'
  | 'like'
  | 'headLike'
  | 'tailLike'
  | 'in'
  | 'notIn'
  | 'gt'
  | 'lt'
  | 'gte'
  | 'lte'

declare type NameEntity<T = {}> = T & {
  id?: number
  name?: string
}
declare type TreeEntity<T = {}> = NameEntity<T> & {
  parentId?: number
  sort?: number
  treePath?: string
}
// interface TreeNode<T = any> extends T, TreeEntity {
//   key?: string | number
//   title?: string
// }
interface ApiError {
  code: number
  msg: string
}
