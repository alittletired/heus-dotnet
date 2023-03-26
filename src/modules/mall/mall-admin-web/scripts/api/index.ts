import path from 'path'
import { ApiConfig } from './api'
import ApiCodeGen from './ApiCodeGen'
const defaultConfig = {
  mapTypes: { Timestamp: 'number', Int64: 'string' },
  codegenType: 'ts',
  pathSplitIndex: 2,
  basePath: '',
  httpImport: '',
  outputDir: 'src/api',
  ignoreTypes: ['DynamicSearch', 'PageList', 'PageRequest'],
}
const apiconfigJson = require(path.resolve('apiconfig.json'))

apiconfigJson.apis.forEach(async (api: ApiConfig) => {
  const config = { ...defaultConfig, ...api }
  const data = await fetch(config.url!)
  const docs = await data.json()
  let codeGen = new ApiCodeGen({ docs, config, classes: {}, models: {} })
  codeGen.generate()
})

//http://localhost:6001/swagger/admin/swagger.json
