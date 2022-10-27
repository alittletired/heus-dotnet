import React, { useState } from 'react'
import { Badge } from 'antd'
import { BellOutlined } from '@ant-design/icons'
const NoticeIcon: React.FC = () => {
  const [count, setCount] = useState(11)
  return (
    <span className="noticeButton">
      <Badge count={count} style={{ boxShadow: 'none' }}>
        <BellOutlined className="icon" />
      </Badge>
      <style jsx global>{`
        .tabs .ant-tabs-nav-list {
          margin: auto;
        }

        .tabs .ant-tabs-nav-scroll {
          text-align: center;
        }
        .tabs .ant-tabs-bar {
          margin-bottom: 0;
        }
      `}</style>
      <style jsx>{`
        .popover {
          position: relative;
          width: 336px;
        }

        .noticeButton {
          display: flex;
          align-items: center;
          height: 100%;
          padding: 0 12px;
          cursor: pointer;
          transition: all 0.3s;
        }
        .icon {
          padding: 4px;
          vertical-align: middle;
        }
      `}</style>
    </span>
  )
}
export default NoticeIcon
