export interface ActionDto {
  name: string
  flag: number
  title: string
  url?: string
}

export interface IDomainEvent {}

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

export interface Resource {
  /** 唯一主键，数据库为varchar(24) */
  id: long
  domainEvents: IDomainEvent[]
  /** 创建人 */
  createdBy?: long
  /** 更新人 */
  updateBy?: long
  /** 创建时间 */
  createdDate?: string
  /** 更新时间 */
  updateDate?: string
  appCode?: string
  name: string
  path: string
  isDeleted: boolean
  type: ResourceType
  sort: number
  treeCode: string
  code: string
  parentId?: long
}

export interface ResourceDto {
  code: string
  name: string
  path: string
  sort?: number
  children?: ResourceDto[]
  actions?: ActionDto[]
}

/** 资源类型 */
export enum ResourceType {
  /** 动作点 */
  Action = 3,
  /** 系统 */
  Application = 0,
  /** 菜单 */
  Menu = 2,
  /** 菜单组 */
  MenuGroup = 1,
}

export const resourceTypeOptions = [
  { title: '动作点', value: 3 },
  { title: '系统', value: 0 },
  { title: '菜单', value: 2 },
  { title: '菜单组', value: 1 },
]

export const getResourceTypeTitle = (resourceType: ResourceType) =>
  resourceTypeOptions.find((o) => o.value === resourceType)?.title
export interface Role {
  /** 唯一主键，数据库为varchar(24) */
  id: long
  domainEvents: IDomainEvent[]
  /** 创建人 */
  createdBy?: long
  /** 更新人 */
  updateBy?: long
  /** 创建时间 */
  createdDate?: string
  /** 更新时间 */
  updateDate?: string
  /** 内置角色，不允许删除 */
  isBuildIn: boolean
  isDeleted: boolean
  /** 角色名 */
  name: string
  /** 角色说明 */
  remarks?: string
}

export interface RoleActionRight {
  /** 唯一主键，数据库为varchar(24) */
  id: long
  domainEvents: IDomainEvent[]
  /** 创建人 */
  createdBy?: long
  /** 更新人 */
  updateBy?: long
  /** 创建时间 */
  createdDate?: string
  /** 更新时间 */
  updateDate?: string
  resourceId: long
  roleId: long
  flag: number
}

export interface User {
  /** 唯一主键，数据库为varchar(24) */
  id: long
  domainEvents: IDomainEvent[]
  /** 创建人 */
  createdBy?: long
  /** 更新人 */
  updateBy?: long
  /** 创建时间 */
  createdDate?: string
  /** 更新时间 */
  updateDate?: string
  /** 用户账号 */
  name: string
  /** 用户手机 */
  phone: string
  nickName: string
  status: UserStatus
  isSuperAdmin: boolean
}

export interface UserActionRight {
  resourcePath: string
  flag: number
}

export interface UserCreateDto {
  name: string
  phone: string
  nickName: string
  plaintextPassword: string
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
  name: string
  phone: string
  nickName: string
  id: long
}

const adminApi = {
  account: {
    login(data: LoginInput): Promise<LoginResult> {
      const path = `/admin/account/login`
      return http.post(path, data)
    },
    sendVerifyCode(phone: string): Promise<boolean> {
      const path = `/admin/account/sendVerifyCode`
      return http.post(path, { params: { phone } })
    },
  },
  resource: {
    syncResources(data: ResourceDto[]): Promise<boolean> {
      const path = `/admin/resource/syncResources`
      return http.post(path, data)
    },
    getUserActionRights(userId: long): Promise<UserActionRight[]> {
      const path = `/admin/resource/getUserActionRights`
      return http.get(path, { params: { userId } })
    },
    delete(id: long): Promise<long> {
      const path = `/admin/resource/delete`
      return http.delete(path, { params: { id } })
    },
    get(id: long): Promise<Resource> {
      const path = `/admin/resource/get`
      return http.get(path, { params: { id } })
    },
    search(data: DynamicSearch<Resource>): Promise<PageList<Resource>> {
      const path = `/admin/resource/search`
      return http.post(path, data)
    },
    update(data: Resource): Promise<Resource> {
      const path = `/admin/resource/update`
      return http.put(path, data)
    },
    create(data: Resource): Promise<Resource> {
      const path = `/admin/resource/create`
      return http.post(path, data)
    },
  },
  role: {
    getActionIds(id: long): Promise<RoleActionRight[]> {
      const path = `/admin/role/getActionIds`
      return http.get(path, { params: { id } })
    },
    authorizeAction(id: long, data: long[]): Promise<boolean> {
      const path = `/admin/role/authorizeAction`
      return http.post(path, data, { params: { id } })
    },
    delete(id: long): Promise<long> {
      const path = `/admin/role/delete`
      return http.delete(path, { params: { id } })
    },
    get(id: long): Promise<Role> {
      const path = `/admin/role/get`
      return http.get(path, { params: { id } })
    },
    search(data: DynamicSearch<Role>): Promise<PageList<Role>> {
      const path = `/admin/role/search`
      return http.post(path, data)
    },
    update(data: Role): Promise<Role> {
      const path = `/admin/role/update`
      return http.put(path, data)
    },
    create(data: Role): Promise<Role> {
      const path = `/admin/role/create`
      return http.post(path, data)
    },
  },
  user: {
    create(data: UserCreateDto): Promise<User> {
      const path = `/admin/user/create`
      return http.post(path, data)
    },
    delete(id: long): Promise<long> {
      const path = `/admin/user/delete`
      return http.delete(path, { params: { id } })
    },
    get(id: long): Promise<User> {
      const path = `/admin/user/get`
      return http.get(path, { params: { id } })
    },
    search(data: DynamicSearch<User>): Promise<PageList<User>> {
      const path = `/admin/user/search`
      return http.post(path, data)
    },
    update(data: UserUpdateDto): Promise<User> {
      const path = `/admin/user/update`
      return http.put(path, data)
    },
  },
}
export default adminApi
