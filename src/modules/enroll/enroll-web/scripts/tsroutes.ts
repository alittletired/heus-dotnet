import fs from 'fs'
// import page from '../src/pages/auth/user'

import path from 'path'
import prettier from 'prettier'
// console.warn(page)

const rootpath = path.resolve('./src/pages')
const routes: string[] = []
function toUpperCamelCase(str: string) {
  return str.replace(/[-_/\\](\w)/g, (a, b) => b.toUpperCase())
}
const pages: string[] = []
function readDir(dir: string) {
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

      pages.push(pagePath)
    }
  }
}
readDir(rootpath)
async function aa() {
  for (let page of pages) {
    let fullPath = path.resolve(__dirname, 'src', 'pages', page)
    let a = require(fullPath).default
  }
}
aa()
