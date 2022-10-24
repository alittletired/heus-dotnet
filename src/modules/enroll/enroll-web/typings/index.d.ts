type ViewType = 'create' | 'update' | 'view'
type Func<T, D = any> = (value?: T) => D | Promise<D>
type FuncSync<T, D = any> = (value?: T) => D
