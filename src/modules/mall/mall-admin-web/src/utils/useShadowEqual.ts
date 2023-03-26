import React, {useEffect, useRef} from 'react'
const hasOwn = Object.prototype.hasOwnProperty

export function useShadowEqual<T extends (...args: any[]) => any>(
  func: T,
  deps: ReadonlyArray<any> = [],
) {
  const refdeps = useRef(deps)
  let ref = useRef(func)

  useEffect(() => {
    for (let idx = 0; idx < deps.length; idx++) {
      if (!shallowEqual(deps[idx], refdeps.current[idx])) {
        ref.current()
        return
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, deps)
  useEffect(() => {
    ref.current = func
    refdeps.current = deps
  })
  useEffect(() => ref.current(), [])
}
export default function shallowEqual(a: any, b: any) {
  if (a === b) {
    return true
  }

  if (typeof a !== 'object' || a === null || typeof b !== 'object' || b === null) {
    return false
  }

  var keysA = Object.keys(a)
  var keysB = Object.keys(b)

  if (keysA.length !== keysB.length) {
    return false
  }
  for (var i = 0; i < keysA.length; i++) {
    // key值相等的时候
    // 借用原型链上真正的 hasOwnProperty 方法，判断ObjB里面是否有A的key的key值
    // 属性的顺序不影响结果也就是{name:'daisy', age:'24'} 跟{age:'24'，name:'daisy' }是一样的
    // 最后，对对象的value进行一个基本数据类型的比较，返回结果
    if (!hasOwn.call(b, keysA[i]) || a[keysA[i]] !== b[keysA[i]]) {
      return false
    }
  }

  return true
}
