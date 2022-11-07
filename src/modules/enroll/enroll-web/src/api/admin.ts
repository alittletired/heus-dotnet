export interface ActionDto {
  name: string
  flag: number
  title: string
  url?: string
}

export interface ActionRight {
  /** 唯一主键，数据库为varchar(24) */
  id: long
  /** 创建人 */
  createdBy?: long
  /** 更新人 */
  updateBy?: long
  /** 创建时间 */
  createdDate?: string
  /** 更新时间 */
  updateDate?: string
  resourceId: long
  name: string
  flag: number
  title: string
  url?: string
  isDeleted: boolean
}

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

export interface Resource {
  /** 唯一主键，数据库为varchar(24) */
  id: long
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

export interface User {
  /** 唯一主键，数据库为varchar(24) */
  id: long
  /** 创建人 */
  createdBy?: long
  /** 更新人 */
  updateBy?: long
  /** 创建时间 */
  createdDate?: string
  /** 更新时间 */
  updateDate?: string
  /** 用户账号 */
  userName: string
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
const adminApi = {
  accounts: {
    login(data: LoginInput): Promise<LoginResult> {
      const path = `/admin/accounts/login`
      return http.post(path, data)
    },
    sendVerifyCode(phone: string): Promise<boolean> {
      const path = `/admin/accounts/sendVerifyCode`
      return http.post(path, { params: { phone } })
    },
  },
  resources: {
    syncResources(data: ResourceDto[]): Promise<boolean> {
      const path = `/admin/resources/syncResources`
      return http.post(path, data)
    },
    getUserActionRights(userId: long): Promise<UserActionRight[]> {
      const path = `/admin/resources/getUserActionRights`
      return http.get(path, { params: { userId } })
    },
    delete: {},
    get(id: long): Promise<Resource> {
      const path = `/admin/resources/get`
      return http.get(path, { params: { id } })
    },
    search(data: DynamicSearch<Resource>): Promise<PageList<Resource>> {
      const path = `/admin/resources/search`
      return http.post(path, data)
    },
    update(data: Resource): Promise<Resource> {
      const path = `/admin/resources/update`
      return http.put(path, data)
    },
    create(data: Resource): Promise<Resource> {
      const path = `/admin/resources/create`
      return http.post(path, data)
    },
  },
  roles: {
    authorizeActionRights(id: long, data: ActionRight[]): Promise<boolean> {
      const path = `/admin/roles/authorizeActionRights`
      return http.post(path, data, { params: { id } })
    },
    delete: {},
    get(id: long): Promise<Role> {
      const path = `/admin/roles/get`
      return http.get(path, { params: { id } })
    },
    search(data: DynamicSearch<Role>): Promise<PageList<Role>> {
      const path = `/admin/roles/search`
      return http.post(path, data)
    },
    update(data: Role): Promise<Role> {
      const path = `/admin/roles/update`
      return http.put(path, data)
    },
    create(data: Role): Promise<Role> {
      const path = `/admin/roles/create`
      return http.post(path, data)
    },
  },
  users: {
    findByUserName(name: string): Promise<ICurrentUser> {
      const path = `/admin/users/findByUserName`
      return http.post(path, { params: { name } })
    },
    delete: {},
    get(id: long): Promise<User> {
      const path = `/admin/users/get`
      return http.get(path, { params: { id } })
    },
    search(data: DynamicSearch<User>): Promise<PageList<User>> {
      const path = `/admin/users/search`
      return http.post(path, data)
    },
    update(data: User): Promise<User> {
      const path = `/admin/users/update`
      return http.put(path, data)
    },
    create(data: User): Promise<User> {
      const path = `/admin/users/create`
      return http.post(path, data)
    },
  },
}
export default adminApi
