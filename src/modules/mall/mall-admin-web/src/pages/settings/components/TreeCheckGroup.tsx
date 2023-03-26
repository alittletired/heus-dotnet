import React, { useRef, useState, useEffect, PropsWithChildren, useMemo } from 'react'

import { Tree, TreeProps, TreeDataNode } from 'antd'
interface Props<T extends TreeEntity> {
  value?: number[]
  onChange?: (value: number[]) => void
  treeData: T[]
}
export default function TreeCheckGroup<T extends TreeEntity>(props: PropsWithChildren<Props<T>>) {
  const onCheck = (_: any, e: any) => {
    let checkedKeys = e.checkedNodes
      ?.filter((node: any) => !node.children)
      .map((node: any) => node.key)
    props.onChange?.(checkedKeys)
  }
  //todo 勾选后默认勾选查看
  const treeData = useMemo(() => {
    let treeMap: Record<string, DataNode> = {}
    let treeData: DataNode[] = []
    props.treeData.forEach((item) => {
      let node = { key: item.id, title: item.name }
      if (item.parentId === 0 || !treeMap[item.parentId]) {
        treeData.push(node)
      } else if (treeMap[item.parentId]) {
        let parent = treeMap[item.parentId]
        parent.children = parent.children || []
        parent.children.push(node)
      }
      treeMap[item.id] = node
    })
    return treeData
  }, [props.treeData])

  return (
    <Tree
      checkable
      autoExpandParent
      defaultExpandAll
      onCheck={onCheck}
      treeData={treeData}
      checkedKeys={props.value}
    />
  )
}
