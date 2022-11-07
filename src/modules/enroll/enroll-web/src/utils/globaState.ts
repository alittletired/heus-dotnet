import { atom, useAtom } from 'jotai'
class GlobaState<T> {
  private state: T
  private setAotms = new Set()
  public constructor(public defaultState: T, public key?: string) {
    this.state = defaultState
    if (typeof window !== 'undefined') {
      const value = this.key && localStorage?.getItem(this.key)
      if (value) {
        this.state = JSON.parse(value)
      }
    }
  }
  public getState = () => this.state
  public setState = (newValue: T) => {
    this.state = newValue
    this.key && localStorage.setItem(this.key, JSON.stringify(newValue))
    this.setAotms.forEach((set: any) => {
      set(this.state)
    })
  }

  public onMount = (setAotm: any) => {
    this.setAotms.add(setAotm)
    return () => this.setAotms.delete(setAotm)
  }
  public resetState = () => {
    this.state = this.defaultState
    this.setAotms.forEach((set: any) => {
      set(this.state)
    })
  }
}

export function withGlobaState<T>(defaultState: T, persistkey?: string) {
  const myStorage = new GlobaState<T>(defaultState, persistkey)
  const baseAtom = atom(myStorage.getState())

  const state = atom(
    (get) => get(baseAtom),
    (get, set, newValue: T) => {
      set(baseAtom, newValue)
      myStorage.setState(newValue)
    },
  )
  baseAtom.onMount = (setAtom) => {
    return myStorage.onMount(setAtom)
  }
  const useState = () => useAtom(state)
  return { ...myStorage, useState, state }
}
