import user from './user'
import { message } from 'antd'
import { AxiosError, AxiosResponse } from 'axios'
import axios from 'axios'
import globalConfig from '@/config/globalConfig'

export const axiosInstance = axios.create({
  timeout: 10000,
  baseURL: globalConfig.apiBaseUrl,
})
const httpClient: HttpClient = {
  get: async function <D, R>(url: string, config?: RequestConfig<D>): Promise<R> {
    const res = await axiosInstance.get(url, config)
    return res.data.data
  },
  post: async function <D, R>(url: string, data?: D, config?: RequestConfig<D>): Promise<R> {
    const res = await axiosInstance.post(url, data, config)

    return res.data.data
  },

  put: async function <D, R>(url: string, data?: D, config?: RequestConfig<D>): Promise<R> {
    const res = await axiosInstance.put(url, data, config)
    return res.data.data
  },
  patch: async function <D, R>(url: string, data?: D, config?: RequestConfig<D>): Promise<R> {
    const res = await axiosInstance.patch(url, data, config)
    return res.data.data
  },
  delete: async function <D, R>(url: string, config?: RequestConfig<D>): Promise<R> {
    return axiosInstance.delete(url, config).then((res) => res.data.data)
  },
}

export function getAuthHeaders(): Record<string, string> {
  if (!user.state.isLogin) {
    return {}
  }
  return {
    Authorization: 'Bearer ' + user.state.accessToken,
    userId: user.state.userId,
  }
}
axiosInstance.interceptors.request.use(async (config) => {
  if (user.state.isLogin) {
    config.headers = Object.assign({}, config.headers, getAuthHeaders())
  }
  return config
})
axiosInstance.interceptors.response.use(
  (res) => {
    const body = res.data
    var code = parseInt(body.code, 10)
    if (code) {
      console.warn('error', res)
      if ([405, 404, 403].includes(body.code)) {
        user.logout()
      }
      return Promise.reject(res)
    }

    return res
  },
  (error: AxiosError) => {
    if (error.response?.status == 401) {
      user.logout()
      if (location.pathname.startsWith(globalConfig.loginUrl)) return
      const redirect = encodeURIComponent(location.href.substring(location.origin.length))
      location.href = location.origin + globalConfig.loginUrl + '?redirect=' + redirect
    } else {
      // message.error('系统出错，请联系管理员')
      console.warn('http error', error, error.message)

      return Promise.reject(error)
    }
  },
)

globalThis.http = httpClient
export function isApiError(ex?: AxiosResponse<ApiError> | any): ex is AxiosResponse<ApiError> {
  return ex?.data?.code && ex?.data?.message
}
export default axiosInstance
