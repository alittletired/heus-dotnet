import { usePrevious } from '@/utils/hook'
import shallowEqual from '@/utils/useShadowEqual'
import { Tree } from 'antd'
import { TreeProps } from 'antd/lib/tree'
import React, { useCallback, useEffect, useRef, useState } from 'react'
import ActionIcon, { ActionIconProps } from '../action/ActionIcon'
import { OnApiBefore } from '../table/types'
import Loading from '../loading'
import './ApiTree.less'
import { toTreeData, TreeEntity, TreeNodeData } from './treeUtils'
export interface ApiTreeInstance<T = any, P = any> {
  reload: () => any
}
export const ApiTreeContext = React.createContext({} as ApiTreeInstance)
export const useApiTree = () => React.useContext(ApiTreeContext)

export interface ApiTreeProps<T extends TreeEntity, P = any> extends Omit<TreeProps, 'onSelect'> {
  api: (p: P) => Promise<T[]>
  data?: P
  onApiBefore?: OnApiBefore<P>
  tree?: ApiTreeInstance<T, P>
  onSelect?: (data: T) => any
  nodeActions?: ActionIconProps<T>[]
}
export default function ApiTree<T extends TreeEntity, P>(props: ApiTreeProps<T, P>) {
  let { api, onSelect, data, nodeActions, onApiBefore, ...rest } = props
  let prevData = useRef(data)
  const [treeData, setTreeData] = useState([] as TreeNodeData<T>[])
  let handleSelect = useCallback(
    async (keys: any, info: any) => {
      onSelect?.(info.node)
    },
    [onSelect],
  )
  const titleRender = useCallback(
    (node: T) => {
      return (
        <span className="api-tree-node">
          <span className="api-tree-node-title">{node.name}</span>
          <span className="api-tree-node-action">
            {nodeActions.map((iconProps) => (
              <ActionIcon
                data={node}
                onSuccess={reload.current}
                key={iconProps.title}
                {...iconProps}
              />
            ))}
          </span>
        </span>
      )
    },
    [nodeActions],
  )
  const reload = useRef(async (data?: any) => {
    data = data ?? prevData.current
    if (onApiBefore(data)) {
      api(data).then((treeData) => setTreeData(toTreeData(treeData)))
    }
  })
  useEffect(() => {
    if (shallowEqual(prevData.current, data)) return
    reload.current(data)
  }, [data])

  if (props.tree) {
    props.tree.reload = reload.current
  }
  useEffect(() => {
    prevData.current = data
  }, [data])
  return (
    <Tree
      titleRender={titleRender}
      onSelect={handleSelect}
      blockNode={true}
      showIcon={true}
      className="api-tree"
      {...rest}
      treeData={treeData}
    />
  )
}
