export interface CodeGen {
    generate(): void
}

export interface ApiConfig {
    name?: string
    pathGlob?: string[]
    url?: string
    pathSplitIndex?: number
    outputDir: string
    ignoreTypes: string[]
    responseWarp?: string
    basePath?: string
}
export interface ApiMethodParamSchema {
    type: string
    in: string
    description?: string
    required?: boolean
}
export interface ApiMethodSchma {
    path: string
    httpMethod: string
    responseType: string
    summary?: string
    params: Record<string, ApiMethodParamSchema>
}
export type ApiClassSchma = { [key: string]: ApiMethodSchma | ApiClassSchma }

export interface ApiModelPropsSchma {
    name: string
    required: boolean
    type: string
    description?: string
    example: string
}
export interface ApiModelSchma {
    description?: string
    properties: Record<string, ApiModelPropsSchma>
    name: string
}
export interface ApiContext<T = any> {
    config: ApiConfig
    classes: ApiClassSchma
    models: Record<string, ApiModelSchma>
    docs: T
}
export interface ApiParser<T = any> {
    parse(apiConext: ApiContext<T>): void
}

export function toLowerCamelCase(str: string) {
    str = toUpperCamelCase(str)
    return str[0].toLowerCase() + str.substr(1)
}

export function toUpperCamelCase(str: string) {
    return str.replace(/[-_](\w)/g, (a, b) => b.toUpperCase())
}

export function normalType(str: string) {
    str = str
        .replace('#/definitions/', '')
        .replace('#/components/schemas/', '')
        .replace(/«/g, '<')
        .replace(/»/g, '>')
    // str = str.replace(/[^List|<List]<([^>]*)>/g, (a, b) => b + '[]')
    return str
}
export function isApiMethodSchma(obj: any): obj is ApiMethodSchma {
    return 'params' in obj && 'responseType' in obj
}
