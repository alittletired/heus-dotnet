import { FormInstance } from 'antd'
import React, { useContext } from 'react'
interface FormContext {
  loading: boolean
  setLoading: (loading: boolean) => void
  onSuccess?: (data: any) => void
  noLabel?: boolean
  form?: FormInstance
}
const FormContext = React.createContext<FormContext>({
  loading: false,
  setLoading(loading: boolean) {},
})
export const useFormContext = () => useContext(FormContext)
export default FormContext
