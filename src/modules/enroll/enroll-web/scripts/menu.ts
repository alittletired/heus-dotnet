import path from 'path'
import fs from 'fs'
import menus from '../src/config/menus'
import appConfig from '../src/config/appConfig'
import { Resource, ResourceDto } from '@/api/admin'
import axios from 'axios'
const appCode = null
const rootpath = path.resolve('./src/pages')
const resources: Record<string, ResourceDto> = {}
async function syncResources() {
  const url = appConfig.apiBaseUrl + '/admin/resources/SyncResource'
  await axios.post(url, Object.values(resources))
}
// function normalizeMenu(subMenus: Menu[], ) {
//   for (const subMenu of subMenus) {
//     let { children, ...rest } = subMenu
//     const resource: ResourceDto = {
//       ...rest,
//       actions: [],
//     }
//     resources[resource.code] = resource
//     normalizeMenu(children || [])
//   }
// }
async function loadPages(dir: string) {
  const files = fs.readdirSync(dir)
  for (const fileName of files) {
    if (fileName === 'components' || fileName.startsWith('_')) {
      continue
    }
    const pagePath = path.join(dir, fileName)
    if (fs.lstatSync(pagePath).isDirectory()) {
      loadPages(pagePath)
      continue
    }
    if (!pagePath.endsWith('.jsx') && !pagePath.endsWith('.tsx')) {
      continue
    }
    console.log(' load page ' + pagePath)
    var pageComponent: PageComponent = await import(pagePath)
    console.log(' load page options' + pageComponent.options)
  }
}

async function main() {
  console.log('start load page rootPath:' + rootpath)
  await loadPages(rootpath)
  // await syncResources()
}
main()
