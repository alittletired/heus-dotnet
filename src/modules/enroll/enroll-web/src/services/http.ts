import { axiosInstance } from '../../scripts/api/httpClient.axios'
import { setHttpClient } from '@/api/admin'
setHttpClient(axiosInstance)
export default axiosInstance
