import axios from 'axios'
import { HttpClient, RequestConfig } from './Template'

export const axiosInstance = axios.create({
  timeout: 10000,
  baseURL: process.env.NEXT_PUBLIC_API_URL,
})
const httpClient: HttpClient = {
  get: function <D, R>(url: string, config?: RequestConfig<D>): Promise<R> {
    return axiosInstance.get(url, config).then((res) => res.data.data)
  },
  post: function <D, R>(url: string, config?: RequestConfig<D>): Promise<R> {
    return axiosInstance.post(url, config).then((res) => res.data.data)
  },
  delete: async function <D, R>(url: string, config?: RequestConfig<D>): Promise<R> {
    return axiosInstance.delete(url, config).then((res) => res.data.data)
  },
  put: function <D, R>(url: string, config?: RequestConfig<D>): Promise<R> {
    return axiosInstance.put(url, config).then((res) => res.data.data)
  },
  patch: function <D, R>(url: string, config?: RequestConfig<D>): Promise<R> {
    return axiosInstance.patch(url, config).then((res) => res.data.data)
  },
}
export default httpClient
