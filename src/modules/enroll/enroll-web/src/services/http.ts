import axios from 'axios'
interface RequestConfig<D> {
  data?: D
  params?: any
}

interface HttpClient {
  get<D, R>(url: string, config?: RequestConfig<D>): Promise<R>
  post<D, R>(url: string, config?: RequestConfig<D>): Promise<R>
  delete<D, R>(url: string, config?: RequestConfig<D>): Promise<R>
  put<D, R>(url: string, config?: RequestConfig<D>): Promise<R>
  patch<D, R>(url: string, config?: RequestConfig<D>): Promise<R>
}

const axiosInstance = axios.create({
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
