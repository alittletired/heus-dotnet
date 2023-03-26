const idKey = 'id'
export function getProperty<T>(data: any, propertName: any) {
  return data[propertName]
}

export function getId(data: any) {
  return data[idKey]
}
export const idIsEqual = (item1: any, item2: any) => {
  return item1.id === item2.id
}

export const setId = (data: any, id: number) => {
  data[idKey] = id
}
export const getIdKey = () => idKey

export function replaceItemAtIndex<T>(arr: T[], index: number, newItem: T) {
  return [...arr.slice(0, index), newItem, ...arr.slice(index + 1)]
}

export function removeItemAtIndex<T>(arr: T[], index: number) {
  return [...arr.slice(0, index), ...arr.slice(index + 1)]
}

const hasOwn = Object.prototype.hasOwnProperty

function is(x: any, y: any) {
  if (x === y) {
    return x !== 0 || y !== 0 || 1 / x === 1 / y
  } else {
    // eslint-disable-next-line
    return x !== x && y !== y
  }
}

export function shallowEqual(objA: any, objB: any) {
  if (is(objA, objB)) return true

  if (
    typeof objA !== 'object' ||
    objA === null ||
    typeof objB !== 'object' ||
    objB === null
  ) {
    return false
  }

  const keysA = Object.keys(objA)
  const keysB = Object.keys(objB)

  if (keysA.length !== keysB.length) return false

  for (let i = 0; i < keysA.length; i++) {
    if (!hasOwn.call(objB, keysA[i]) || !is(objA[keysA[i]], objB[keysA[i]])) {
      return false
    }
  }

  return true
}
