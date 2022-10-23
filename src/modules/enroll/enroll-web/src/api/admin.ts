interface RequestConfig<D> {
  data?: D
  params?: any
}

interface HttpClient {
  get<D, R>(url: string, config?: RequestConfig<D>): Promise<R>
  post<D, R>(url: string, config?: RequestConfig<D>): Promise<R>
  delete<D, R>(url: string, config?: RequestConfig<D>): Promise<R>
  put<D, R>(url: string, config?: RequestConfig<D>): Promise<R>
  patch<D, R>(url: string, config?: RequestConfig<D>): Promise<R>
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

export interface AuthTokenDto {
  userId?: long
  accessToken?: string
  expiration?: long
}

export interface ICurrentUser {
  id?: long
  userName?: string
}

export interface LoginInput {
  userName?: string
  password?: string
  rememberMe?: boolean
}

export interface PagedList<T> {
  count?: number
  items?: T[]
}

export interface RestPasswordDto {
  userId?: long
  newPassword?: string
}

export interface RoleCreateDto {
  id?: long
  createdBy?: long
  updateBy?: long
  createdDate?: string
  updateDate?: string
}

export interface RoleDto {
  isBuildIn?: boolean
  isDeleted?: boolean
  name?: string
  remarks?: string
  id?: long
  createdBy?: long
  updateBy?: long
  createdDate?: string
  updateDate?: string
}

export interface RoleUpdateDto {}

export interface UserCreateDto {
  account?: string
  password?: string
  phone?: string
  salt?: string
  status?: UserStatus
  id?: long
  createdBy?: long
  updateBy?: long
  createdDate?: string
  updateDate?: string
}

export interface UserDto {
  userName?: string
  account?: string
  password?: string
  phone?: string
  salt?: string
  status?: UserStatus
  id?: long
  createdBy?: long
  updateBy?: long
}

/** 用户状态 */
export interface UserStatus {
  name?: string
  value?: number
}

export interface UserUpdateDto {
  id?: long
}

const adminApi = {
  accounts: {
    login(data: LoginInput): Promise<AuthTokenDto> {
      const path = `/admin/accounts/login`
      return httpClient.post(path, { data })
    },
    refreshToken(data: AuthTokenDto): Promise<AuthTokenDto> {
      const path = `/admin/accounts/refreshToken`
      return httpClient.post(path, { data })
    },
    sendVerifyCode(phone: string): Promise<boolean> {
      const path = `/admin/accounts/sendVerifyCode`
      return httpClient.post(path, { params: { phone } })
    },
  },
  roles: {
    get(id: long): Promise<RoleDto> {
      const path = `/admin/roles/get`
      return httpClient.get(path, { params: { id } })
    },
    query(data: DynamicQuery<RoleDto>): Promise<PagedList<RoleDto>> {
      const path = `/admin/roles/query`
      return httpClient.post(path, { data })
    },
    create(data: RoleCreateDto): Promise<RoleDto> {
      const path = `/admin/roles/create`
      return httpClient.post(path, { data })
    },
    update(data: RoleUpdateDto): Promise<RoleDto> {
      const path = `/admin/roles/update`
      return httpClient.put(path, { data })
    },
    delete: {},
    getResourceIds(id: long): Promise<long[]> {
      const path = `/admin/roles/getResourceIds`
      return httpClient.get(path, { params: { id } })
    },
    authorizeResources(id: long, data: long[]): Promise<boolean> {
      const path = `/admin/roles/authorizeResources`
      return httpClient.post(path, { data, params: { id } })
    },
  },
  users: {
    get(id: long): Promise<UserDto> {
      const path = `/admin/users/get`
      return httpClient.get(path, { params: { id } })
    },
    query(data: DynamicQuery<UserDto>): Promise<PagedList<UserDto>> {
      const path = `/admin/users/query`
      return httpClient.post(path, { data })
    },
    create(data: UserCreateDto): Promise<UserDto> {
      const path = `/admin/users/create`
      return httpClient.post(path, { data })
    },
    delete: {},
    update(data: UserUpdateDto): Promise<UserDto> {
      const path = `/admin/users/update`
      return httpClient.put(path, { data })
    },
    getUserRoleIds(id: long): Promise<long[]> {
      const path = `/admin/users/getUserRoleIds`
      return httpClient.get(path, { params: { id } })
    },
    resetPassword(data: RestPasswordDto): Promise<boolean> {
      const path = `/admin/users/resetPassword`
      return httpClient.post(path, { data })
    },
    findByUserName(name: string): Promise<ICurrentUser> {
      const path = `/admin/users/findByUserName`
      return httpClient.post(path, { params: { name } })
    },
  },
}
export default adminApi
