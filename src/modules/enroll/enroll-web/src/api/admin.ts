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

export interface ICurrentUser {
  id?: long
  userName: string
}

export interface LoginInput {
  userName: string
  password: string
  rememberMe: boolean
}

export interface LoginResult {
  userId: long
  nickName: string
  accessToken: string
  expiration: long
}

export interface PagedList<T> {
  total: number
  items: T[]
}

export interface Resource {
  /** 唯一主键，数据库为varchar(24) */
  id: long
  /** 创建人 */
  createdBy?: long
  /** 更新人 */
  updateBy?: long
  /** 创建时间 */
  createdDate: string
  /** 更新时间 */
  updateDate: string
  type: ResourceType
  code: string
  isDeleted: boolean
  sort: number
  treeCode: string
  treePath: string
  parentId?: long
}

/** 资源类型 */
export enum ResourceType {
  /** 动作点 */
  Action = 1,
  /** 系统 */
  Application = 0,
  /** 菜单 */
  Menu = 1,
  /** 菜单组 */
  MenuGroup = 1,
}

export const resourceTypeOptions = [
  { title: '动作点', value: 1 },
  { title: '系统', value: 0 },
  { title: '菜单', value: 1 },
  { title: '菜单组', value: 1 },
]

export const getResourceTypeTitle = (resourceType: ResourceType) =>
  resourceTypeOptions.find((o) => o.value === resourceType)?.title
export interface RestPasswordDto {
  userId: long
  newPassword: string
}

export interface RoleCreateDto {
  id?: long
  createdBy?: long
  updateBy?: long
  createdDate: string
  updateDate?: string
}

export interface RoleDto {
  isBuildIn: boolean
  isDeleted: boolean
  name?: string
  remarks?: string
  id?: long
  createdBy?: long
  updateBy?: long
  createdDate: string
  updateDate?: string
}

export interface RoleUpdateDto {}

export interface UserCreateDto {
  account?: string
  password?: string
  phone?: string
  salt?: string
  status: UserStatus
  id?: long
  createdBy?: long
  updateBy?: long
  createdDate: string
  updateDate?: string
}

export interface UserDto {
  userName: string
  account?: string
  password?: string
  phone?: string
  salt?: string
  status: UserStatus
  id?: long
  createdBy?: long
  updateBy?: long
}

/** 用户状态 */
export enum UserStatus {
  /** 禁用 */
  Disabled = 1,
  /** 锁定 */
  Locked = 3,
  /** 正常 */
  Normal = 0,
  /** 不存在 */
  NotFound = 4,
  /** 未激活 */
  Unactivated = 2,
}

export const userStatusOptions = [
  { title: '禁用', value: 1 },
  { title: '锁定', value: 3 },
  { title: '正常', value: 0 },
  { title: '不存在', value: 4 },
  { title: '未激活', value: 2 },
]

export const getUserStatusTitle = (userStatus: UserStatus) =>
  userStatusOptions.find((o) => o.value === userStatus)?.title
export interface UserUpdateDto {
  id: long
}

const adminApi = {
  accounts: {
    login(data: LoginInput): Promise<LoginResult> {
      const path = `/admin/accounts/login`
      return httpClient.post(path, data)
    },
    sendVerifyCode(phone: string): Promise<boolean> {
      const path = `/admin/accounts/sendVerifyCode`
      return httpClient.post(path, { params: { phone } })
    },
  },
  resources: {
    create(data: Resource): Promise<Resource> {
      const path = `/admin/resources/create`
      return httpClient.post(path, data)
    },
    delete: {},
    query(data: DynamicQuery<Resource>): Promise<PagedList<Resource>> {
      const path = `/admin/resources/query`
      return httpClient.post(path, data)
    },
    update(data: Resource): Promise<Resource> {
      const path = `/admin/resources/update`
      return httpClient.put(path, data)
    },
  },
  roles: {
    get(id: long): Promise<RoleDto> {
      const path = `/admin/roles/get`
      return httpClient.get(path, { params: { id } })
    },
    query(data: DynamicQuery<RoleDto>): Promise<PagedList<RoleDto>> {
      const path = `/admin/roles/query`
      return httpClient.post(path, data)
    },
    create(data: RoleCreateDto): Promise<RoleDto> {
      const path = `/admin/roles/create`
      return httpClient.post(path, data)
    },
    update(data: RoleUpdateDto): Promise<RoleDto> {
      const path = `/admin/roles/update`
      return httpClient.put(path, data)
    },
    delete: {},
    getResourceIds(id: long): Promise<long[]> {
      const path = `/admin/roles/getResourceIds`
      return httpClient.get(path, { params: { id } })
    },
    authorizeResources(id: long, data: long[]): Promise<boolean> {
      const path = `/admin/roles/authorizeResources`
      return httpClient.post(path, data, { params: { id } })
    },
  },
  users: {
    get(id: long): Promise<UserDto> {
      const path = `/admin/users/get`
      return httpClient.get(path, { params: { id } })
    },
    query(data: DynamicQuery<UserDto>): Promise<PagedList<UserDto>> {
      const path = `/admin/users/query`
      return httpClient.post(path, data)
    },
    create(data: UserCreateDto): Promise<UserDto> {
      const path = `/admin/users/create`
      return httpClient.post(path, data)
    },
    delete: {},
    update(data: UserUpdateDto): Promise<UserDto> {
      const path = `/admin/users/update`
      return httpClient.put(path, data)
    },
    getUserRoleIds(id: long): Promise<long[]> {
      const path = `/admin/users/getUserRoleIds`
      return httpClient.get(path, { params: { id } })
    },
    resetPassword(data: RestPasswordDto): Promise<boolean> {
      const path = `/admin/users/resetPassword`
      return httpClient.post(path, data)
    },
    findByUserName(name: string): Promise<ICurrentUser> {
      const path = `/admin/users/findByUserName`
      return httpClient.post(path, { params: { name } })
    },
    getResourceCodes(userId: long): Promise<string[]> {
      const path = `/admin/users/getResourceCodes`
      return httpClient.get(path, { params: { userId } })
    },
  },
}
export default adminApi
