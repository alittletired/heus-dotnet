interface PageRequest {
  pageIndex?: number
  pageSize?: number
  orderBy?: string
}
type DynamicSearch<T> = {
  [P in keyof T]?: T[P] | { ['$' + key in Operator]?: T[P] | Array<T[P]> }
} & PageRequest
interface PageList<T> {
  total: number
  items: T[]
}
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

interface RequestConfig<D> {
  data?: D
  params?: any
}

interface HttpClient {
  get<D, R>(url: string, config?: RequestConfig<D>): Promise<R>
  post<D, R>(url: string, data?: D, config?: RequestConfig<D>): Promise<R>
  delete<D, R>(url: string, config?: RequestConfig<D>): Promise<R>
  put<D, R>(url: string, data?: D, config?: RequestConfig<D>): Promise<R>
  patch<D, R>(url: string, data?: D, config?: RequestConfig<D>): Promise<R>
}
type long = string
declare var http: HttpClient

type ControlLabels<T> = { [key: keyof T]: string } & Record<string, string>

interface ApiResult<T> {
  code: number
  message?: string
  data?: T
}
interface ApiError {
  code: number
  message: string
}
