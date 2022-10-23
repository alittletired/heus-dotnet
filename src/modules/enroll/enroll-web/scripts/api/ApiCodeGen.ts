import {
  ApiClassSchma,
  ApiContext,
  ApiMethodSchma,
  ApiModelSchma,
  ApiParser,
  isApiMethodSchma,
  toLowerCamelCase,
} from './api'
import path from 'path'
import OpenapiParser from './OpenapiParser'
import fs from 'fs'
import prettier from 'prettier'
import SwaggerParser from './SwaggerParser'
const httpMethods = ['get', 'post', 'put', 'delete', 'patch']
export default class ApiCodeGen {
  constructor(protected apiContext: ApiContext) {}
  generate() {
    let { docs } = this.apiContext
    let { name, url } = this.apiContext.config
    console.log('start generate ', name, url)
    let parser: ApiParser
    if (docs.swagger) {
      parser = new SwaggerParser()
    } else {
      parser = new OpenapiParser()
    }
    parser.parse(this.apiContext)
    this.buildFile()
  }
  protected generateModel(arr: string[], modelSchma: ApiModelSchma) {
    let { config } = this.apiContext
    let typeName = modelSchma.name
    // if (typeName.startsWith(config.responseWarp)) continue
    //排除Map定义
    if (typeName === 'Map<T>') return
    if (config.ignoreTypes.some((i) => i === typeName)) return
    let titles: Record<string, string> = {}
    if (modelSchma.description) {
      arr.push(`/** ${modelSchma.description} */`)
    }
    arr.push(`export interface ${typeName} {`)
    for (let propertyName in modelSchma.properties) {
      let property = modelSchma.properties[propertyName]
      let { description, title } = property
      description = description || title
      if (description && description.trim() !== propertyName) {
        arr.push(`/** ${description} */`)
      }

      let name = propertyName.indexOf('-') > -1 ? "'" + propertyName + "'" : propertyName
      if (title) {
        titles[name] = title
      }
      arr.push(`${name}${property.required ? '' : '?'}: ${property.type}`)
    }

    arr.push('}\n')
    if (Object.keys(titles).length) {
      arr.push(`export const ${toLowerCamelCase(typeName)}Titles=${JSON.stringify(titles)}`)
    }
  }
  protected generateEnum(arr: string[], modelSchma: ApiModelSchma) {
    let { properties, name: enumName, description } = modelSchma
    if (description) {
      arr.push(`/** ${description} */`)
    }
    arr.push(`export enum ${enumName}{`)
    let propNames = Object.keys(properties).sort(
      (a, b) => parseInt(properties[a].example) - parseInt(properties[b].example),
    )
    for (let propertyName of propNames) {
      let property = properties[propertyName]
      arr.push(`/** ${property.title} */`)
      arr.push(`${propertyName}=${property.default},`)
    }
    arr.push('}\n')

    const enumOptionName = `${toLowerCamelCase(enumName)}Options`
    arr.push(`export const ${enumOptionName}=[\n`)
    for (let propertyName of propNames) {
      let property = properties[propertyName]
      arr.push(`{title:'${property.title}',value:${property.default}},`)
    }
    arr.push(`]\n`)
    let paramName = toLowerCamelCase(enumName)
    arr.push(
      `export const get${enumName}Title=(${paramName}:${enumName})=>${enumOptionName}.find(o=>o.value===${paramName})?.title`,
    )
  }
  protected buildFile() {
    let { models, config } = this.apiContext
    var template = fs.readFileSync(__dirname + '/Template.ts').toString()
    let tsContent: string[] = [template]
    let typeNames = Object.keys(models).sort()
    // 生成ts类型
    for (let name of typeNames) {
      let modelSchma = models[name]
      if (modelSchma.format == 'enum') {
        this.generateEnum(tsContent, modelSchma)
      } else {
        this.generateModel(tsContent, modelSchma)
      }
    }

    let options = prettier.resolveConfig.sync(path.join(process.cwd(), 'prettier.config.js'))
    const apiDir = path.join(process.cwd(), config.outputDir)
    if (!fs.existsSync(apiDir)) {
      fs.mkdirSync(apiDir)
    }
    let content = tsContent.join('\n') + this.generateApi()
    let finalContent = prettier.format(config.httpImport + '\n' + content, {
      ...options,
      parser: 'typescript',
    })
    fs.writeFileSync(path.join(apiDir, config.name + `.ts`), finalContent, 'utf8')
  }
  protected generateApi() {
    let { classes, config } = this.apiContext
    //生成api
    let arr: string[] = []
    let classNames = Object.keys(classes)

    //如果只有一个顶层，则不生成嵌套
    if (classNames.length === 1) {
      this.generateClass(classes[classNames[0]] as ApiClassSchma, arr)
    } else {
      this.generateClass(classes, arr)
    }
    let api = `\n const ${config.name}Api={${arr.join('\n')}}`

    return api + `\nexport default ${config.name}Api`
  }
  protected generateClass(allclasss: ApiClassSchma, arr: string[]) {
    for (let className in allclasss) {
      let apiClass = allclasss[className]

      if (isApiMethodSchma(apiClass)) {
        this.generateMethod(arr, apiClass, className)
      } else {
        let keys = Object.keys(apiClass)
        if (keys.length === 1 && httpMethods.includes(keys[0])) {
          let apiMethod = apiClass[keys[0]]
          if (isApiMethodSchma(apiMethod)) {
            this.generateMethod(arr, apiMethod, className)
          }
        } else {
          arr.push(`${className}: {`)
          this.generateClass(apiClass, arr)
          arr.push(`},`)
        }
      }
    }
  }
  protected generateMethod(arr: string[], apiMothod: ApiMethodSchma, methodName: string) {
    let { config } = this.apiContext

    let { path, httpMethod, responseType, summary, params } = apiMothod

    let queryParamArr: string[] = []
    let bodyParamArr: string[] = []
    let pathParams: string[] = []
    for (let paramName in params) {
      let param = params[paramName]
      if (param.in === 'path') {
        let pathStr = ''
        if (param.description && paramName !== param.description) {
          pathStr += `/** ${param.description} */\n`
        }
        pathStr += `${paramName}${param.required ? '' : '?'}: ${param.type}`
        pathParams.push(pathStr)
      } else if (param.in === 'query') {
        let queryStr = ''

        if (param.description && paramName !== param.description) {
          queryStr += `/** ${param.description} */`
        }
        queryStr += `${paramName}${param.required ? '' : '?'}: ${param.type}`
        queryParamArr.push(queryStr)
      } else if (param.in === 'body') {
        if (bodyParamArr.length > 0) {
          console.error('不能同时存在两个body', path)
          continue
        }

        bodyParamArr.push(`data${param.required ? '' : '?'}: ${param.type}`)
      }
    }
    if (summary && methodName !== summary) {
      arr.push(`/**`)
      arr.push(`*${summary}`)
      arr.push(`*/`)
    }
    let methodStr = `${methodName}(`
    let paramArrs: string[] = []
    if (pathParams.length > 0) {
      paramArrs = paramArrs.concat(pathParams)
    }
    if (queryParamArr.length > 0) {
      if (queryParamArr.length === 1) {
        paramArrs.push(queryParamArr.join('\n').replace('?', ''))
      } else {
        paramArrs.push(`params:{${queryParamArr.join('\n')}}`)
      }
    }
    if (bodyParamArr.length > 0) {
      paramArrs.push(bodyParamArr.join(''))
    }

    methodStr += paramArrs.join(',') + ')'

    if (config.responseWarp && responseType.startsWith(config.responseWarp)) {
      const rgResponseWarp = new RegExp(`^${config.responseWarp}<(.*)>`)
      responseType = responseType.replace(rgResponseWarp, (a, b) => b)
    }
    methodStr += `: Promise<${responseType}> {`
    methodStr += `const path = \`${config.basePath}${path.replace(/\{/g, '${')}\`\n`
    methodStr += `return  httpClient.${httpMethod}(path,{`
    if (bodyParamArr.length > 0) {
      methodStr += 'data,'
    }
    if (queryParamArr.length === 1) {
      let queryStr = queryParamArr[0].split('*/')
      let paramName = queryStr[queryStr.length - 1].split(':')[0].replace('?', '')
      methodStr += `params:{${paramName}}`
    } else if (queryParamArr.length > 0) {
      methodStr += 'params'
    } else {
      methodStr += ''
    }
    // methodStr += ').then(res => res.data.data'
    // if (config.responseWarp) {
    //   methodStr += '.data'
    // }
    methodStr += '})},'
    arr.push(methodStr)
  }
}
