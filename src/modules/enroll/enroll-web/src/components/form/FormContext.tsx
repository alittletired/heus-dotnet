import { FormInstance } from 'antd'
import React from 'react'
interface FormContext {
  loading: boolean
  setLoading: (loading: boolean) => void
  onSuccess?: (data: any) => void
  noLabel?: boolean
  form?: FormInstance
  labels?: { [key in string]: string }
}
const FormContext = React.createContext<FormContext>({
  loading: false,
  setLoading(loading: boolean) {},
})
export default FormContext
