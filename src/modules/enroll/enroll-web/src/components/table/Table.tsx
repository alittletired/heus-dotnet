import React, {
  useState,
  useEffect,
  useCallback,
  useMemo,
  useRef,
  Dispatch,
  SetStateAction,
} from 'react'
import { Table, Card, TablePaginationConfig } from 'antd'
import { toSearchData, translateColumns } from './tableUtils'
import { ColumnProps, TableProps } from './interface'
import SearchForm from './SearchForm'
import ToolBar from './ToolBar'
import { usePageContext } from '../PageContext'
// import './index.css'
import { toTreeData, TreeEntity } from '../tree/treeUtils'
export interface TableContext<T = any> {
  props: TableProps<T>
  columns: ColumnProps<T>[]
  dataSource: T[]
  setDataSource: Dispatch<SetStateAction<T[]>>
  reset: () => Promise<void>
  //当前的请求参数
  search: (data?: Partial<T>) => Promise<void>
}

export const TableContext = React.createContext({} as TableContext)
export const useTable = () => React.useContext(TableContext)
export default function ApiTable<T extends object>(props: TableProps<T>) {
  const [dataSource, setDataSource] = useState([] as T[])
  const [total, setTotal] = useState(0)
  const [loading, setLoading] = useState(false)
  const pageRef = useRef<PageRequest>({
    pageIndex: 1,
    pageSize: props.pagination === false ? -1 : props.pagination?.pageSize || 10,
  })
  const pageContext = usePageContext()
  const dataRef = useRef({ ...props.data })
  const propsRef = useRef(props)
  propsRef.current = props
  const loadingRef = useRef(loading)
  const columnsRef = useRef([] as ColumnProps[])
  const columns = useMemo(() => {
    let columns = translateColumns(props.columns)
    columns.forEach((col) => {
      if (col.dataIndex && !col.title) {
        col.title =
          props.titles?.[col.dataIndex.toString()] ?? pageContext.labels?.[col.dataIndex.toString()]
      }
      if (!col.title) {
        console.warn('列没有设置标题:', col)
      }
      if (col.actions) {
        col.actions.forEach((action) => {
          if (!action.title && action.actionName) {
            action.title =
              action.title ??
              props.titles?.[action.actionName] ??
              pageContext.labels?.[action.actionName] ??
              action.actionName
          }
        })
      }
    })
    columnsRef.current = columns
    return columns
  }, [pageContext.labels, props.columns, props.titles])
  const fetchData = useRef(async (data?: Partial<T>, pageRequest?: PageRequest) => {
    if (loadingRef.current) return

    if (!propsRef.current.fetchApi) return
    try {
      loadingRef.current = true
      setLoading(true)
      let page = pageRequest || pageRef.current
      dataRef.current = { ...dataRef.current, ...data }
      if (propsRef.current.beforeFetch) {
        let finalData = await propsRef.current.beforeFetch(dataRef.current)
        if (finalData == false) return
        dataRef.current = finalData
      }
      let searchData = toSearchData(dataRef.current, columnsRef.current, page)

      let { total, items: dataSource } = await propsRef.current.fetchApi!(searchData)

      dataSource.forEach(
        (item: any, index: number) =>
          (item._index = (page.pageIndex! - 1) * page.pageSize! + index + 1),
      )

      if (props.useTreeTable) {
        //@ts-ignore
        dataSource = toTreeData(dataSource)
      }

      setDataSource(dataSource)
      setTotal(total)
    } finally {
      loadingRef.current = false
      setLoading(false)
    }
  })

  const onTableChange = useCallback((page: TablePaginationConfig, filters: any, sorter: any) => {
    let pageRequest: PageRequest = { pageSize: page.pageSize, pageIndex: page.current }
    if (sorter) {
      if (sorter.order) {
        pageRequest.orderBy = `${sorter.field} ${sorter.order === 'descend' ? 'desc' : 'asc'}`
      } else {
        pageRequest.orderBy = undefined
      }
    }
    fetchData.current(dataRef.current, pageRequest)
  }, [])

  useEffect(() => {
    let propData = { ...props.data }

    for (let key of Object.keys(propData)) {
      if (key in dataRef.current) {
        //@ts-ignore
        if (propParams[key] !== params[key]) {
          fetchData.current(propData)
          return
        }
      } else {
        fetchData.current(propData)
        return
      }
    }
  }, [props.data])

  const search = useCallback(async (p?: Partial<T>) => {
    await fetchData.current(p, pageRef.current)
  }, [])
  const reset = useCallback(async () => {
    dataRef.current = { ...props.data }
    pageRef.current.pageIndex = 1
    await fetchData.current()
  }, [props.data])
  useEffect(() => {
    fetchData.current()
  }, [])
  const tableContext: TableContext<T> = {
    props,
    dataSource,
    setDataSource,
    columns,
    reset,
    search,
  }
  let pageConfig = {
    current: pageRef.current.pageIndex,
    total: total,
    showSizeChanger: true,
    hideOnSinglePage: true,
    ...props.pagination,
  }
  return (
    <TableContext.Provider value={tableContext}>
      <div className="api-table">
        <SearchForm />
        <Card
          bodyStyle={{ height: '100%', padding: 0 }}
          title={props.tableTitle}
          extra={<ToolBar items={props.toolBar} />}>
          <Table
            scroll={{ x: 'max-content' }}
            key={dataSource.length}
            rowKey={props.rowKey ?? 'id'}
            size="small"
            expandable={{ defaultExpandAllRows: true }}
            {...props}
            loading={loading}
            onChange={onTableChange}
            dataSource={dataSource}
            pagination={pageRef.current.pageSize! > 0 && pageConfig}
            columns={columns}
          />
        </Card>
        <style jsx global>{`
          .api-table {
            flex: auto;
            height: 100%;
          }
          .api-table .ant-card {
            flex: auto;
            height: 100%;
          }
          .api-table .ant-card-body {
            height: 100%;
            padding: 0px;
            position: relative;
            overflow: auto;
            border-bottom: 1px solid #f0f0f0;
            margin-top: 1px;
          }
          .api-table td.column {
            padding-right: 12px;
          }
          .api-table .search {
            margin-bottom: 16px;
            padding: 24px 24px 0px;
            background: #fff;
          }
          .api-table .search-col {
            padding-left: 8px;
            padding-right: 8px;
          }
          .api-table .search-button {
            margin-right: 8px;
          }

          .api-table .toobar {
            display: flex;
            align-items: center;
            justify-content: space-between;
          }
          .api-table .toolbar-title {
            flex: 1 1;
            color: rgba(0, 0, 0, 0.85);
            font-weight: 500;
            font-size: 16px;
            line-height: 24px;
            opacity: 0.85;
          }
          .api-table .toolbar-option {
            display: flex;
            align-items: center;
            justify-content: flex-end;
          }

          .api-table .ant-table-tbody > tr > td:not(.table-index-column) {
            min-width: 90px;
          }
          .toolbar-option button {
            margin-left: 12px;
          }

          .api-table .date-column {
            width: 110px;
          }
          .api-table .datetime-column {
            width: 150px;
          }
          .api-table .column {
            min-width: 80px;
          }
        `}</style>
      </div>
    </TableContext.Provider>
  )
}

// const ForwardApiTable = React.forwardRef(ApiTable)
// ForwardPageTable.IndexColumn = IndexColumn
// todo: 没找到使用functioncompoent泛型并支持ref的方法,故使用class包裹一层
