import axios from 'axios'
import path from 'path'
import {ApiConfig} from './api'
import ApiCodeGen from './ApiCodeGen'

let defaultConfig = {
  mapTypes: {Timestamp: 'number', Int64: 'string'},
  codegenType: 'ts',
  pathSplitIndex: 2,
  basePath: '',
  httpImport: '',
  outputDir: 'src/api',
  ignoreTypes: ['DynamicQuery', 'PageList', 'PageRequest'],
}
let config = require(path.resolve('apiconfig.json'))
let {apis = [], ...restConfig} = {...defaultConfig, ...config}

apis.forEach(async (api: ApiConfig) => {
  let config = {...restConfig, ...api}
  let docs = (await axios.get(config.url)).data
  let codeGen = new ApiCodeGen({docs, config, classes: {}, models: {}})
  codeGen.generate()
})
