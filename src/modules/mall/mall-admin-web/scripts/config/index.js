//@ts-check

const baseConfig = require('./config.base.json')
const path = require('path')
const prettier = require(path.resolve('./node_modules/prettier'))
const fs = require('fs')
const _ = require('lodash')

function buildConfig() {
  let config = Object.assign({}, baseConfig)
  const env = process.argv[2] || 'dev'
  console.log(`merge configuration file config.${env}.json`)
  const envConfig = require(`./config.${env}.json`)
  config = _.merge(config, envConfig)
  config.env = env

  buildPlatform('web', config)
}

function buildPlatform(platform, config) {
  let platformConfig = {}
  // platformConfig = traversalNode(config, platform)
  platformConfig = config
  const envpath = path.join(__dirname, '..', '..', config.output[platform])
  if (!fs.existsSync(envpath)) {
    fs.mkdirSync(envpath)
  }
  const output = path.join(envpath, 'config.ts')
  platformConfig = Object.assign(
    {
      desc: '自动生成的配置文件，不要编辑本文件！！！',
    },
    platformConfig,
  )
  const content = ['export default ', JSON.stringify(platformConfig)].join('\n')
  let options = prettier.resolveConfig.sync(
    path.join(process.cwd(), 'prettier.config.js'),
  )
  const fileContent = prettier.format(content, {
    ...options,
    parser: 'typescript',
  })

  fs.writeFileSync(output, fileContent, 'utf8')
}

function traversalNode(node, platform) {
  if (typeof node !== 'object') return node
  const {ios, web, android, ...newNode} = node
  if (node[platform]) {
    if (typeof node[platform] !== 'object') {
      return node[platform]
    }

    Object.assign(newNode, {
      ...node[platform],
    })
  }
  for (const key in newNode) {
    newNode[key] = traversalNode(newNode[key], platform)
  }
  return newNode
}
buildConfig()
