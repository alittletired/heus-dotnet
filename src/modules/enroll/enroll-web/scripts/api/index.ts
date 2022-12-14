import path from 'path'
import http from 'http'
import fs from 'fs'
import url from 'node:url'
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
  const url = new URL(config.url!)
  const req = http.request(url, (res) => {
    var str = ''
    res.on('data', function (chunk) {
      str += chunk
    })

    res.on('end', function () {
      // fs.writeFileSync('D:\\log\\1.txt', str)
      var docs = JSON.parse(str)
      let codeGen = new ApiCodeGen({ docs, config, classes: {}, models: {} })
      codeGen.generate()
    })
  })

  req.end()
})

//http://localhost:6001/swagger/admin/swagger.json
