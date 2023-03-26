import React, {ReactNode, useCallback, useMemo, useRef} from 'react'

import {Card, Layout} from 'antd'
import './ApiSideTree.less'
import ApiTree, {ApiTreeProps, useApiTree} from './ApiTree'
import {ActionButtonProps} from '../action/ActionButton'
import ActionAnchor from '../action/ActionAnchor'
const {Content, Sider} = Layout
interface SideTreeProps<T, P> extends ApiTreeProps<T, P> {
  sideWidth?: number
  className?: string
  title?: string
  children?: ReactNode
  toolBar?: ActionButtonProps<T>[]
  searchBar?: ReactNode
}
export default function SideTree<T, P>(props: SideTreeProps<T, P>) {
  let tree = useApiTree()
  let {className, sideWidth = 292, toolBar, children, ...treeProps} = props
  let extra = useMemo(() => {
    if (!toolBar) return null
    return toolBar.map((itemProps) => (
      <ActionAnchor
        key={itemProps.title.toString()}
        {...itemProps}
        onSuccess={() => {
          tree.reload()
        }}
      />
    ))
  }, [toolBar, tree])
  return (
    <Layout className={`api-side-tree ${className}`}>
      <Sider width={sideWidth} style={{background: '#fff'}}>
        {props.searchBar}
        <Card
          headStyle={{backgroundColor: '#f8f8fa'}}
          title={props.title}
          bordered={false}
          extra={extra}>
          <ApiTree {...treeProps} tree={tree} />
        </Card>
      </Sider>
      <Content className="right">{children}</Content>
    </Layout>
  )
}
