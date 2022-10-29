export interface RequestConfig<D> {
  data?: D
  params?: any
}

export interface HttpClient {
  get<D, R>(url: string, config?: RequestConfig<D>): Promise<R>
  post<D, R>(url: string, data?: D, config?: RequestConfig<D>): Promise<R>
  delete<D, R>(url: string, config?: RequestConfig<D>): Promise<R>
  put<D, R>(url: string, data?: D, config?: RequestConfig<D>): Promise<R>
  patch<D, R>(url: string, data?: D, config?: RequestConfig<D>): Promise<R>
}

let httpClient: HttpClient
export function setHttpClient(client: HttpClient) {
  httpClient = client
}

type long = string

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

interface PageRequest {
  pageIndex?: number
  pageSize?: number
  orderBy?: string
}
type DynamicQuery<T> = {
  [P in keyof T]?: T[P] | { [key in Operator as `$${key}`]?: T[P] | Array<T[P]> }
} & PageRequest
