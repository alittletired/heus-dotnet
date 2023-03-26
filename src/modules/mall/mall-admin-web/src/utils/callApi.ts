export default async function callApi<D = {}, T = {}>(
  apiProps: ApiProps<D, T>,
  data?: D,
): Promise<T | undefined> {
  let {api, onSuccess, onFail, onBefore} = apiProps
  data = Object.assign({}, apiProps.data, data)
  if (onBefore) {
    let beforeData = await onBefore(data)
    if (typeof beforeData == 'boolean' || !beforeData) {
      return
    } else {
      data = beforeData
    }
  }

  try {
    let output = await api(data)
    //解决组件过快被销毁的问题
    setTimeout(() => onSuccess && onSuccess(output), 16)

    return output
  } catch (ex) {
    if (onFail) onFail(ex)
    else throw ex
  }
}
