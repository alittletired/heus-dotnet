import React, { useState, useCallback } from 'react'
import { Form, Col, Row, Button, Space } from 'antd'
import { DownOutlined, UpOutlined } from '../icons'
import { ColumnProps } from './interface'
import { FormItem } from '../form'
import { useTable } from './Table'
/**
 * 默认的查询表单配置
 */
const defaultColConfig = {
  xs: 24,
  sm: 24,
  md: 12,
  lg: 8,
  xl: 6,
  xxl: 6,
}

type SearchItemPorps<T> = { column: ColumnProps<T> }
export function SearchItem<T>(props: SearchItemPorps<T>) {
  let { column } = props
  let table = useTable()
  let controlProps = { label: column.title as string, name: column.dataIndex as string }
  if (column.options) {
    return (
      <FormItem.Select
        {...controlProps}
        options={column.options}
        mode={column.operator === 'in' ? 'multiple' : undefined}
        hasAllOption={column.operator !== 'in'}
      />
    )
  }
  // return (
  //   <Form.Item>
  //     <Input />
  //   </Form.Item>
  // )
  return <FormItem.Input {...controlProps} />
}
const SearchForm: React.FC = (props) => {
  const table = useTable()
  let [form] = Form.useForm()
  const [expand, setExpand] = useState(false)
  const reset = useCallback(() => {
    form.resetFields()
    table.search({ ...form.getFieldsValue() })
  }, [table, form])
  const searchColumns = table.columns.filter((c) => c.operator && c.dataIndex)
  if (searchColumns.length === 0) return null
  const span = 8
  const rowNumber = 24 / span
  let searchItems: any[] = []
  let showCount = expand
    ? searchColumns.length
    : Math.max(1, Math.min(rowNumber - 1, searchColumns.length))

  for (let index = 0; index < showCount; index++) {
    searchItems.push(
      <Col span={span} className="search-col" key={`col_${index}`}>
        <SearchItem column={searchColumns[index]} />
      </Col>,
    )
  }

  const expandSpan = () => {
    if (searchColumns.length < rowNumber) return null
    return (
      <Button type="link" style={{ fontSize: 12 }} onClick={() => setExpand(!expand)}>
        {expand ? '收起' : '展开'}
        {expand ? <UpOutlined /> : <DownOutlined />}
      </Button>
    )
  }
  const onSearch = () => {
    var data = form.getFieldsValue()
    console.warn('onSearch', data)

    table.search(data)
  }
  const onReset = () => {
    form.resetFields()
    onSearch()
  }
  return (
    <Form form={form} className="search" {...props} onFinish={onSearch}>
      <Row>
        {searchItems}
        <Col span={span} key="sumbitButton" className="search-col">
          <FormItem.Item>
            <Space>
              <Button type="primary" htmlType="submit">
                查询
              </Button>
              <Button onClick={onReset}>重置</Button>
              {expandSpan()}
            </Space>
          </FormItem.Item>
        </Col>
      </Row>
    </Form>
  )
}
export default SearchForm
