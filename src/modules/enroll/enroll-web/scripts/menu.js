//@ts-check
const path = require('path')
const fs = require('fs')
const axios = require(path.resolve('./node_modules/axios'))
const prettier = require(path.resolve('./node_modules/prettier'))

const rootpath = path.resolve('./src/pages')
/** @type {{ name: string;key:string; icon: string; type: number; path: string; children: any[]; }[]} */
let menuGroup = []
/**
 * @returns {Promise<{ id:number
   name: string
	type: number
  key:string
	icon:string
  path: string
  parentId: number}[]>}
 */
const getResources = async () => {
  let res = await axios.get('http://localhost:8080/api/admin/resources/pb/get-list')
  return res.data.data
}
const menuMap = new Map()

const buildMenu = async () => {
  let resources = await getResources()
  for (let resource of resources) {
    let {name, path, type, icon, parentId} = resource

    // @ts-ignore
    let menu = {name, icon, key: name, type, path, children: []}
    if (resource.type === 3) continue
    menuMap.set(resource.id, menu)
    let parent = menuMap.get(parentId)
    if (parent == null) {
      parent = menu
      menu.key = menu.name
      menuGroup.push(menu)
    } else {
      menu.key = parent.name + '.' + menu.name
      parent.children.push(menu)
      if (resource.type === 2) {
        delete menu.children
      }
    }
  }
  let fileContent = '//自动生成代码\n'
  fileContent += `export default ${JSON.stringify(menuGroup)}`
  let options = prettier.resolveConfig.sync(path.resolve('prettier.config.js'))
  fileContent = prettier.format(fileContent, {...options, parser: 'typescript'})
  fs.writeFileSync(path.join(rootpath, 'menus.ts'), fileContent, 'utf8')
}
buildMenu()
