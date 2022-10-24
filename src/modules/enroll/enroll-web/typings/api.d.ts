interface PageRequest {
  pageIndex?: number
  pageSize?: number
  orderBy?: string
}
type DynamicQuery<T> = {
  [P in keyof T]?: T[P] | { ['$' + key in Operator]?: T[P] | Array<T[P]> }
} & PageRequest
type PageList<T> = { items: T[]; total: number }
type Api<T = any, D = any> = (data: T) => Promise<D>
interface ApiProps<T = any, D = any> {
  api: Api<T, D>
  data?: T
  onBefore?: (data: T) => Promise<T | boolean> | T | boolean
  onSuccess?: (data: D) => any
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
