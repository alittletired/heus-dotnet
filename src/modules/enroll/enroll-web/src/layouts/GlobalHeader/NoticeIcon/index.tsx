import React, {useState} from 'react'
import {Badge} from 'antd'
import {BellOutlined} from '@ant-design/icons'
import styles from './index.module.less'
const NoticeIcon: React.FC = () => {
  const [count, setCount] = useState(11)
  return (
    <span className={styles.noticeButton}>
      <Badge count={count} style={{boxShadow: 'none'}}>
        <BellOutlined className={styles.icon} />
      </Badge>
    </span>
  )
}
export default NoticeIcon
