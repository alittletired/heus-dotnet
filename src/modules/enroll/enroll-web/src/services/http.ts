import { getUser, logout } from './user'
import { message } from 'antd'
import { AxiosError } from 'axios'
import axios from 'axios'

export const axiosInstance = axios.create({
  timeout: 10000,
  baseURL: process.env.NEXT_PUBLIC_API_URL,
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

axiosInstance.interceptors.request.use(async (config) => {
  const auth = getUser()

  if (auth.isLogin) {
    config.headers = Object.assign({}, config.headers, {
      Authorization: 'Bearer ' + auth.accessToken,
      userId: auth.userId,
    })
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
        logout()
      }
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

globalThis.http = httpClient
export default axiosInstance
