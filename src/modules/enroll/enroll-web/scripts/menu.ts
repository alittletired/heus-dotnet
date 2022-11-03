import path from 'path'
import menus, { Menu } from '../src/config/menus'
import appConfig from '../src/config/appConfig'
import { Resource } from '@/api/admin'
import axios from 'axios'
const appCode = null
async function syncMenu() {
  const url = appConfig.apiBaseUrl + '/admin/resources/SyncResource'
  const resources: Record<string, Resource> = {}
  normalizeMenu(menus, resources)
  await axios.post(url, Object.values(resources))
}
function normalizeMenu(subMenus: Menu[], resources: Record<string, Resource>, parent?: Resource) {
  for (const subMenu of subMenus) {
    let { children, sort, ...rest } = subMenu
    let treeCode = rest.code
    if (parent) treeCode = parent.treeCode + '.' + treeCode
    const resource: Resource = {
      ...rest,
      id: rest.code,
      treeCode,
      sort: sort ?? 0,
      isDeleted: false,
    }
    resources[resource.code] = resource
    normalizeMenu(children || [], resources, resource)
  }
}
syncMenu()
