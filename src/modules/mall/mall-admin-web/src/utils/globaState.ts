import { atom, PrimitiveAtom, useAtom, WritableAtom } from 'jotai'
import { atomWithStorage } from 'jotai/utils'
import { getDefaultStore } from 'jotai/vanilla'
const defaultStore = getDefaultStore()
class GlobaState<T> {
  private state: WritableAtom<T, any>
  private currentState: T
  public constructor(public defaultState: T, public key?: string) {
    this.currentState = defaultState
    if (key) {
      this.state = atomWithStorage(key, defaultState)
    } else {
      this.state = atom(defaultState)
    }

    defaultStore.sub(this.state, () => {
      this.currentState = defaultStore.get(this.state)
    })
  }
  public useState = () => useAtom(this.state)
  public getState = () => this.currentState
  public setState = (value: T) => defaultStore.set(this.state, value)
}
export function withGlobaState<T>(defaultState: T, persistkey?: string) {
  return new GlobaState(defaultState, persistkey)
}
