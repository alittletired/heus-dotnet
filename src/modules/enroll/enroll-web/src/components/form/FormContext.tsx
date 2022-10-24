import React from 'react'
import {FormInstance} from 'antd/lib/form'
interface FormContext {
  loading: boolean
  setLoading: (loading: boolean) => void
  onSuccess(data: any): void
  noLabel?: boolean
  form?: FormInstance
  titles?: {[key in string]: string}
}
const FormContext = React.createContext<FormContext>({
  loading: false,
  setLoading(loading: boolean) {},
  onSuccess(data: any) {},
})
export default FormContext
