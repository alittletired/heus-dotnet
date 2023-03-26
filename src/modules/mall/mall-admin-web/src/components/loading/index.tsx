import React, {useState, useEffect, useRef, PropsWithChildren} from 'react'
import {Spin} from 'antd'
type LoadingProps = PropsWithChildren<{loadData: () => Promise<any>}>

const Loading: React.FC<LoadingProps> = (props) => {
  const [loaded, setLoaded] = useState(false)
  const [error, setError] = useState('')
  const fetchData = useRef(() => {
    setLoaded(false)
    setError('')
    props
      .loadData()
      .catch((ex: any) => {
        setError(ex)
      })
      .finally(() => {
        setLoaded(true)
      })
  })
  useEffect(() => fetchData.current(), [])
  if (!loaded) return <Spin />
  else {
    return <>{props.children}</>
  }
}
export default Loading
