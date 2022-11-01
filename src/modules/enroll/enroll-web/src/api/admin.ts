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
      return http.post(path, data)
    },
    sendVerifyCode(phone: string): Promise<boolean> {
      const path = `/admin/accounts/sendVerifyCode`
      return http.post(path, { params: { phone } })
    },
  },
  resources: {
    create(data: Resource): Promise<Resource> {
      const path = `/admin/resources/create`
      return http.post(path, data)
    },
    delete: {},
    search(data: DynamicSearch<Resource>): Promise<PageList<Resource>> {
      const path = `/admin/resources/search`
      return http.post(path, data)
    },
    update(data: Resource): Promise<Resource> {
      const path = `/admin/resources/update`
      return http.put(path, data)
    },
  },
  roles: {
    get(id: long): Promise<RoleDto> {
      const path = `/admin/roles/get`
      return http.get(path, { params: { id } })
    },
    search(data: DynamicSearch<RoleDto>): Promise<PageList<RoleDto>> {
      const path = `/admin/roles/search`
      return http.post(path, data)
    },
    create(data: RoleCreateDto): Promise<RoleDto> {
      const path = `/admin/roles/create`
      return http.post(path, data)
    },
    update(data: RoleUpdateDto): Promise<RoleDto> {
      const path = `/admin/roles/update`
      return http.put(path, data)
    },
    delete: {},
    getResourceIds(id: long): Promise<long[]> {
      const path = `/admin/roles/getResourceIds`
      return http.get(path, { params: { id } })
    },
    authorizeResources(id: long, data: long[]): Promise<boolean> {
      const path = `/admin/roles/authorizeResources`
      return http.post(path, data, { params: { id } })
    },
  },
  users: {
    get(id: long): Promise<UserDto> {
      const path = `/admin/users/get`
      return http.get(path, { params: { id } })
    },
    search(data: DynamicSearch<UserDto>): Promise<PageList<UserDto>> {
      const path = `/admin/users/search`
      return http.post(path, data)
    },
    create(data: UserCreateDto): Promise<UserDto> {
      const path = `/admin/users/create`
      return http.post(path, data)
    },
    delete: {},
    update(data: UserUpdateDto): Promise<UserDto> {
      const path = `/admin/users/update`
      return http.put(path, data)
    },
    getUserRoleIds(id: long): Promise<long[]> {
      const path = `/admin/users/getUserRoleIds`
      return http.get(path, { params: { id } })
    },
    resetPassword(data: RestPasswordDto): Promise<boolean> {
      const path = `/admin/users/resetPassword`
      return http.post(path, data)
    },
    findByUserName(name: string): Promise<ICurrentUser> {
      const path = `/admin/users/findByUserName`
      return http.post(path, { params: { name } })
    },
    getResourceCodes(userId: long): Promise<string[]> {
      const path = `/admin/users/getResourceCodes`
      return http.get(path, { params: { userId } })
    },
  },
}
export default adminApi
