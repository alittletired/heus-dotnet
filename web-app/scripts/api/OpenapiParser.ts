import minimatch from 'minimatch'
import { OpenAPIV3 } from './openapi-types'
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
export default class OpenapiParser implements ApiParser<OpenAPIV3.Document> {
    apiConext = {} as ApiContext<OpenAPIV3.Document>

    parse(apiConext: ApiContext<OpenAPIV3.Document>) {
        this.apiConext = apiConext
        let { config, docs } = this.apiConext
        const { pathGlob } = config
        for (let path in docs.paths) {
            if (pathGlob) {
                let matchs = pathGlob.filter((glob) => minimatch(path, glob))
                if (matchs.length !== pathGlob.length) continue
            }
            try {
                this.parsePath(path, docs.paths[path]!)
            } catch (err) {
                console.error(`${path}`)
                throw err
            }
        }
    }

    protected parsePath(path: string, pathInfo: OpenAPIV3.PathItemObject) {
        let pathParts = path.split('/').slice(this.apiConext.config.pathSplitIndex)
        pathParts = pathParts.filter((p) => p[0] !== '{')
        let operations: [string, OpenAPIV3.OperationObject | undefined][] = [
            ['get', pathInfo.get],
            ['post', pathInfo.post],
            ['put', pathInfo.put],
            ['patch', pathInfo.patch],
            ['delete', pathInfo.delete],
        ]
        for (let operationPair of operations) {
            let [httpMethod, operation] = operationPair
            if (!operation) continue
            let { parameters = [], responses, requestBody } = operation
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
                console.error(
                    `同名的方法签名${path} ${apiMethod.path}没有定义200返回对象,将不生成`
                )
                continue
            }
            if (!responses) {
                console.error(`${path} 没有定义返回对象,将不生成`)
                continue
            }
            let response = responses[200]!
            //@ts-ignore
            if (!response || !response.content) {
                console.error(`${path} 没有定义200返回对象,将不生成`)

                continue
            }

            let responseType: string = ''
            if ('$ref' in response) {
                responseType = this.getType(response)
            } else if ('*/*' in response.content!) {
                responseType = this.getType((response.content as any)['*/*'].schema)
            }
            if (!responseType) {
                console.warn(`${path} 没有定义返回值`)
            }
            let params: Record<string, ApiMethodParamSchema> = {}
            if (requestBody && 'content' in requestBody) {
                let body = requestBody.content['application/json'].schema!
                params['requestBody'] = {
                    type: this.getType(body),
                    in: 'body',
                    required: true,
                }
            }
            for (let p of parameters) {
                if ('in' in p) {
                    let { in: paramIn, name } = p
                    if (paramIn === 'header' || paramIn === 'formData') {
                        continue
                    }
                    let type: string = ''
                    if ('$ref' in p) {
                        type = this.getType(p)
                    } else {
                        type = this.getType(p.schema as any)
                    }
                    try {
                        params[name] = { ...p, type }
                    } catch (ex) {
                        ex.message = path + '\n' + ex.message
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

    protected getType(
        property: OpenAPIV3.SchemaObject | OpenAPIV3.ReferenceObject
    ): string {
        if ('$ref' in property) return this.getRefType(property.$ref)
        if ('type' in property) {
            let type = property.type
            switch (type) {
                case 'integer':
                case 'number':
                    return 'number'
                case 'string':
                    return 'string'
                case 'boolean':
                    return 'boolean'
                case 'array':
                    if ('items' in property) {
                        let refType = this.getType(property.items)
                        return `${refType}[]`
                    }
                    throw new Error(`数组解析出错${property}`)

                case 'object':
                    if (property.additionalProperties) return 'any'
                    throw new Error(`无法识别的object类型${JSON.stringify(property)}`)
                default:
                    throw new Error(
                        `无法识别的属性类型 ${type} ${JSON.stringify(property)}`
                    )
            }
        }
        throw new Error(`不识别的对象${property}`)
    }

    protected checkRefType(name: string, definition: OpenAPIV3.SchemaObject) {
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
            description: definition.description,
            properties: {},
            name: finalName,
        }

        //先插入 防止递归引用类型
        let hasIgnore = config.ignoreTypes.some(
            (ignore) => ignore === finalName || finalName.startsWith(ignore + '<')
        )
        if (!hasIgnore) {
            models[finalName] = apiModel
        }

        let required = definition.required || []
        for (let propName in definition.properties) {
            let property = definition.properties[propName]
            try {
                let tsType = this.getType(property)
                if (tsType === genericName || tsType === genericName + '[]') {
                    tsType = tsType.replace(genericName, 'T')
                }
                let {
                    description = '',
                    example = '',
                } = property as OpenAPIV3.BaseSchemaObject
                apiModel.properties[propName] = {
                    name,
                    required: required.includes(propName),
                    type: tsType,
                    description,
                    example,
                }
            } catch (ex) {
                console.error(ex)
                throw new Error(
                    `无法识别类型${propName} property:${JSON.stringify(property)}`
                )
            }
        }
    }

    protected getRefType(refType: string): string {
        let tsType = normalType(refType)
        let definition = this.apiConext.docs.components?.schemas?.[
            refType.replace('#/components/schemas/', '')
        ]
        if (!definition) throw new Error(`${refType} is null`)

        if ('$ref' in definition) {
            this.getType(definition)
        } else {
            this.checkRefType(tsType, definition)
        }
        return tsType
    }
}
