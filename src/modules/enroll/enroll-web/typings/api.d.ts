interface PageRequest {
  pageIndex?: number
  pageSize?: number
  orderBy?: string
}
type BaseOperator = 'eq' | 'neq' | 'gt' | 'lt' | 'gte' | 'lte' | 'like' | 'headLike' | 'tailLike'
type CollectionOperator = 'in' | 'notIn'
type Operator = BaseOperator | CollectionOperator
type DynamicSearchFilter<T, P extends keyof T> =
  | { op: CollectionOperator; value: Array<T[P]> }
  | { op: BaseOperator; value: T[P] }

type DynamicSearch<T> = {
  filters: { [P in keyof T]?: T[P] | DynamicSearchFilter<T, P> }
} & PageRequest
interface PageList<T> {
  total: number
  items: T[]
}
type IsValidArg<T> = T extends object ? (keyof T extends never ? false : true) : true

type ApiRequest<T> = T extends (...args: any) => Promise<infer R>
  ? {
      request: T
      onBefore?: (data: ApiDataType<T>) => Promise<ApiDataType<T> | boolean>
      onSuccess?: (data: R) => any
      onFail?(err: any): void
      // params?: ApiRequestParamType<T>
    } & ApiRequestParams<T>
  : never

type ApiRequestParams<T> = T extends (p: infer P, d: infer D) => any
  ? D extends Object
    ? { params: P }
    : {}
  : {}

type ApiRequestParamsType<T> = T extends (p: infer P, d: infer D) => any
  ? D extends Object
    ? P
    : never
  : never
type ApiDataType<T> = T extends (p: infer P, d: infer D) => any ? (D extends Object ? D : P) : p

type ApiParamRequest<D, R> = (params: any, data: D) => Promise<R>
interface RequestProps<D, R> {
  request?: ApiRequest<D, R> | ApiParamRequest<D, R>
  onBefore?: (data: D) => Promise<D | boolean> | D | boolean
  onSuccess?: (data: R) => any
  onFail?(err: any): void
}

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

type Labels<T> = { [key: keyof T]: string } & Record<string, string>

interface ApiResult<T> {
  code: number
  message?: string
  data?: T
}
interface ApiError {
  code: number
  message: string
}
