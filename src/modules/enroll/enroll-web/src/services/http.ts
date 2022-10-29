import { axiosInstance } from '../../scripts/api/httpClient.axios'
import { setHttpClient } from '@/api/admin'
import { logout } from './user'
import { message } from 'antd'
import { AxiosError } from 'axios'
setHttpClient(axiosInstance)
axiosInstance.interceptors.response.use(
  (res) => {
    const body = res.data
    if ([405, 404, 403].includes(body.code)) {
      logout()
      // store.dispatch({ type: "SIGN_OUT" })
      throw res
    } else if (parseInt(body.code, 10) !== 200) {
      console.warn('error', res)
      message.error(body.message)
      return Promise.reject(res)
    }
    return res
  },
  (error: AxiosError) => {
    if (error.response?.status == 401) {
      logout()
    }
    // message.error('系统出错，请联系管理员')
    console.warn('http error', error, error.message)

    return Promise.reject(error)
  },
)
export default axiosInstance
