import { atom, AtomEffect, selector, useRecoilState, useRecoilValue } from 'recoil'
class MyStorage<T> {
  private _key: string
  private _defaultValue: T
  private _value: T
  public constructor(key: string, defaultValue: T) {
    this._key = key
    this._defaultValue = defaultValue
    this._value = this._defaultValue
    if (typeof window !== 'undefined') {
      const value = localStorage?.getItem(this._key)
      if (value) {
        this._value = JSON.parse(value)
      }
    }
  }
  private _handlers: Array<(value: T) => void> = []
  public addListener(handler: (value: T) => void) {
    this._handlers.push(handler)
  }

  public removeListener(handler: (value: T) => void) {
    this._handlers = this._handlers.filter((h) => h != handler)
  }
  public set = (newValue: T) => {
    this._value = newValue
    localStorage.setItem(this._key, JSON.stringify(newValue))
  }
  public triggerChange = (value: T) => {
    this.set(value)
    this._handlers.forEach((handler) => handler(value))
  }
  public get = () => {
    return this._value
  }
  public reset = () => {
    this._value = this._defaultValue
    localStorage.removeItem(this._key)
  }
}

export function withStorage<T>(key: string, defaultValue: T) {
  const myStorage = new MyStorage<T>(key, defaultValue)
  const effect: AtomEffect<T> = ({ setSelf, trigger, onSet }) => {
    if (trigger === 'get') {
      setSelf(myStorage.get())
    }
    myStorage.addListener(setSelf)
    onSet((newValue, _, isReset) => {
      isReset ? myStorage.reset() : myStorage.set(newValue)
    })
    return () => {
      myStorage.removeListener(setSelf)
    }
  }
  const state = atom({
    key,
    effects: [effect],
  })
  return { ...myStorage, state }
}
