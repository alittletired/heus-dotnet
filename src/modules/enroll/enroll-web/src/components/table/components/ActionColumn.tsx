import React, { useCallback } from 'react'
import { ToolBarItem } from '../types'
import { Divider, Button, Menu, Dropdown } from 'antd'
import { DownOutlined } from '@ant-design/icons'
import { useTable } from '../Table'
import ActionAnchor from '../../action/ActionAnchor'
import { usePageContext } from '../../PageContext'
import { useAuth } from '@/services/auth'
interface Props<T> {
  actions?: ToolBarItem<T>[]
  data: any
  showCount?: number
}
export default function ActionColumn<T>(props: Props<T>) {
  let { actions = [], data, showCount = 3 } = props
  const table = useTable()
  const auth = useAuth()
  actions = actions.filter((action) => auth.hasRight(action.actionName))
  const moreDom: JSX.Element[] = []
  const actionDom: JSX.Element[] = []
  for (let index = 0; index < actions.length; index++) {
    let action = actions[index]
    let { autoReload, disabled, ...rest } = action
    const item = (
      <ActionAnchor
        {...rest}
        onSuccess={() => autoReload !== false && table.search()}
        data={data}
        key={index}
      />
    )
    if (index < showCount - 1 && index < actions.length - 1) {
      actionDom.push(item)
      actionDom.push(<Divider type="vertical" key={`divi_${index}`} />)
    } else if (actions.length <= showCount) {
      actionDom.push(item)
    } else {
      moreDom.push(item)
    }
  }
  if (moreDom.length) {
    const menu = (
      <Menu>
        {moreDom.map((item, index) => (
          <Menu.Item key={index}>{item}</Menu.Item>
        ))}
      </Menu>
    )
    return (
      <>
        {actionDom}
        <Dropdown overlay={menu}>
          <Button type="link" className="ant-dropdown-link">
            更多
            <DownOutlined />
          </Button>
        </Dropdown>
      </>
    )
  }
  return <>{actionDom}</>
}
