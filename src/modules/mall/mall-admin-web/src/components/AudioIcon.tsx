import React from 'react'
import {PlayCircleOutlined} from '@ant-design/icons'
function playAudio(audioUrl: string) {
  let audio = new Audio()
  audio.src = audioUrl
  audio.play()
}
const AudioIcon: React.FC<{src: string}> = (props) => {
  if (!props.src) return null
  return (
    <PlayCircleOutlined
      style={{marginRight: 12}}
      size={20}
      alt="播放"
      onClick={() => playAudio(props.src)}
    />
  )
}
export default AudioIcon
