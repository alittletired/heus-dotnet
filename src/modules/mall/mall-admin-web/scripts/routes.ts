//@ts-check
import fs from 'fs'
import path from 'path'

import prettier from 'prettier'
const rootpath = path.resolve('./src/pages')

/**
 * @param {string} s
 */
function toUrl(s: string) {
  let urlPart = s.split('/')
  urlPart = urlPart.map((u) =>
    u.replace(/(?:^|\.?)([A-Z])/g, (x, y) => '-' + y.toLowerCase()).replace(/^-/, ''),
  )
  return urlPart.join('/')
}

function toUpperCamelCase(str: string) {
  return str.replace(/[-_/\\](\w)/g, (a, b) => b.toUpperCase())
}

const routes: string[] = []

const imports: string[] = []

async function readDir(dir: string) {
  const files = fs.readdirSync(dir)
  for (const fileName of files) {
    if (fileName === 'components' || fileName.startsWith('_')) {
      continue
    }

    const pagePath = path.join(dir, fileName)
    if (fs.lstatSync(pagePath).isDirectory()) {
      readDir(pagePath)
    } else {
      if (!fileName.endsWith('.jsx') && !fileName.endsWith('.tsx')) {
        continue
      }
      let pageImport = pagePath.substring(rootpath.length, pagePath.length - 4).replace(/\\/g, '/')
      if (pageImport.endsWith('/index')) {
        pageImport = pageImport.substring(0, pageImport.length - 6)
      }
      let pageName = toUpperCamelCase(pageImport)
      imports.push(`import ${pageName} from '../pages${pageImport}'`)
      console.warn('pageImport', pageName, pageImport)

      // routes.push(`'${publicUrl}/${toUrl(pageImport.substr(1))}':${pageName}`)
    }
  }
}
readDir(rootpath)
// let fileContent = '//自动生成代码\n'
// fileContent += `${imports.join('\n')}\nconst routes= {${routes.join(',\n')}}`
// fileContent += '\nexport type RouteKey =keyof typeof routes\nexport default routes'
// let options = prettier.resolveConfig.sync(path.resolve('prettier.config.js'))
// fileContent = prettier.format(fileContent, {...options, parser: 'typescript'})
// fs.writeFileSync(path.join(rootpath, 'routes.ts'), fileContent, 'utf8')
// require('./menu')
