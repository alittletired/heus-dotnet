import { OpenAPIV2 } from './openapi-types'
import {
  ApiClassSchma,
  ApiContext,
  ApiMethodParamSchema,
  ApiMethodSchma,
  ApiModelSchma,
  ApiParser,
  normalType,
  toLowerCamelCase,
} from './api'

export default class SwaggerParser implements ApiParser<OpenAPIV2.Document> {
  apiConext = {} as ApiContext<OpenAPIV2.Document>

  parse(apiConext: ApiContext<OpenAPIV2.Document>) {
    this.apiConext = apiConext
    let { config, docs } = this.apiConext

    for (let path in docs.paths) {
      this.parsePath(path, docs.paths[path])
    }
  }
  protected parsePath(path: string, pathInfo: OpenAPIV2.PathItemObject) {
    let pathParts = path.split('/').slice(this.apiConext.config.pathSplitIndex)
    pathParts = pathParts.filter((p) => p[0] !== '{')
    let operations: [string, OpenAPIV2.OperationObject | undefined][] = [
      ['get', pathInfo.get],
      ['post', pathInfo.post],
      ['put', pathInfo.put],
      ['patch', pathInfo.patch],
      ['delete', pathInfo.delete],
    ]
    for (let operationPair of operations) {
      let [httpMethod, operation] = operationPair
      if (!operation) continue

      let { parameters = [], responses } = operation
      let apiClasses = this.apiConext.classes
      let methodParts = [...pathParts]
      for (let i = 0; i < methodParts.length; i++) {
        let pathPart = methodParts[i]
        let name = toLowerCamelCase(pathPart)
        if (!apiClasses[name]) {
          apiClasses[name] = {}
        }
        apiClasses = apiClasses[name] as ApiClassSchma
      }
      let apiMethod = apiClasses[httpMethod] as ApiMethodSchma
      if (apiMethod) {
        throw new Error('同名的方法签名' + path + ' ' + apiMethod.path)
      }

      if (typeof responses[200].schema != 'object') {
        console.warn(`${path} 没有定义返回值`)
      }
      let responseType = this.getType(responses[200].schema)
      let params: Record<string, ApiMethodParamSchema> = {}
      for (let p of parameters) {
        if ('in' in p) {
          let { in: paramIn, name } = p
          if (paramIn === 'header' || paramIn === 'formData') {
            continue
          }
          try {
            let type = this.getType(p)
            params[name] = { ...p, type }
          } catch (ex) {
            console.error(path)
            throw ex
          }
        }
      }

      apiClasses[httpMethod] = apiMethod = {
        ...operation,
        httpMethod,
        responseType,
        path,
        params,
      }
    }
  }

  protected getType(property: any): string {
    let { type, $ref, schema } = property
    if (schema) return this.getType(schema)
    if ($ref) return this.getRefType($ref)
    switch (type) {
      case 'integer':
      case 'number':
      case 'long':
      case 'int64':
      case 'int':
        return 'number'
      case 'string':
        return 'string'
      case 'boolean':
        return 'boolean'
      case 'list':
      case 'array':
        let refType = this.getType(property.items)
        return `${refType}[]`
      case 'object':
        if (property.additionalProperties) return 'any'
        throw new Error(`无法识别的object类型${JSON.stringify(property)}`)
      default:
        throw new Error(`无法识别的属性类型 ${type} ${JSON.stringify(property)}`)
    }
  }

  protected checkRefType(name: string, schema: OpenAPIV2.SchemaObject) {
    let { models, config } = this.apiConext
    if (models[name]) return
    let genericName = ''
    if (name.includes('<')) {
      genericName = name
        .substring(name.indexOf('<') + 1, name.lastIndexOf('>'))
        .replace('[', '')
        .replace(']', '')
    }
    let finalName = name.replace(/<.*>$/, '<T>').replace('[', '').replace(']', '')

    let apiModel: ApiModelSchma = {
      description: schema.description,
      properties: {},
      name: finalName,
    }

    //先插入 防止递归引用类型
    let hasIgnore = config.ignoreTypes.some(
      (ignore) => ignore === finalName || finalName.startsWith(ignore + '<'),
    )
    if (!hasIgnore) {
      models[finalName] = apiModel
    }

    let required = schema.required || []
    for (let propName in schema.properties) {
      let property = schema.properties[propName]
      try {
        let tsType = this.getType(property)
        if (tsType === genericName || tsType === genericName + '[]') {
          tsType = tsType.replace(genericName, 'T')
        }

        apiModel.properties[propName] = {
          ...property,
          name,
          nullable: !required.includes(propName),
          type: tsType,

          example: property.example,
        }
      } catch (ex) {
        console.error(ex)
        throw new Error(`无法识别类型${propName} property:${JSON.stringify(property)}`)
      }
    }
  }

  protected getRefType(refType: string): string {
    let tsType = normalType(refType)
    let definition = this.apiConext.docs.definitions![refType.replace('#/definitions/', '')]
    this.checkRefType(tsType, definition)
    return tsType
  }
}
