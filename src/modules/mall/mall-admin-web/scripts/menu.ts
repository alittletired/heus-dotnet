import path from 'path'
import fs from 'fs'
import menus from '../src/config/menus'
import appConfig from '../src/config/siteSettings'
import { Resource, ResourceDto } from '@/api/admin'
import * as ts from 'typescript'
import axios from 'axios'
const appCode = null
const rootpath = path.resolve('./src/pages')
const tsOptions: ts.CompilerOptions = {
  allowJs: true,
  jsx: ts.JsxEmit.Preserve,
  module: ts.ModuleKind.CommonJS,
}
const resources: Record<string, ResourceDto> = {}
async function syncResources() {
  const url = appConfig.apiBaseUrl + '/admin/resources/SyncResources'
  await axios.post(url, menus)
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

    // const sourceFile = program.getSourceFile(pagePath)
    // const printer = ts.createPrinter({ newLine: ts.NewLineKind.LineFeed })

    // if (pagePath.includes('login.tsx')) {
    //   const source = fs.readFileSync(pagePath).toString()
    //   console.log('transplate', source)
    //   var result = ts.transpileModule(source, {
    //     compilerOptions: { module: ts.ModuleKind.ESNext, jsxFactory: 'h', jsx: ts.JsxEmit.React },
    //   })
    //   console.log('result', result)
    // }
  }
}

async function main() {
  console.log('start load page rootPath:' + rootpath)
  await loadPages(rootpath)
  await syncResources()
}
main()
