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
// import './index.css'
import { toTreeData, TreeEntity } from '../tree/treeUtils'
export interface TableContext<T = any, P = any> {
  props: TableProps<T, P>
  columns: ColumnProps<T>[]
  dataSource: T[]
  setDataSource: Dispatch<SetStateAction<T[]>>
  params: P
  //当前的请求参数
  setParams: Dispatch<SetStateAction<P & PageRequest>>
}

export const TableContext = React.createContext({} as TableContext)
export const useTable = () => React.useContext(TableContext)
export default function ApiTable<T extends object = any, P = {}>(props: TableProps<T, P>) {
  let [dataSource, setDataSource] = useState([] as T[])
  let [loading, setLoading] = useState(false)
  let [params, setParams] = useState(
    () =>
      ({
        ...props.params,
        pageIndex: 1,
        pageSize: props.pagination === false ? -1 : props.pagination?.pageSize || 10,
      } as P & PageRequest),
  )

  let pageOptions = useMemo<TablePaginationConfig>(() => {
    return {
      pageSize: params.pageSize,
      current: params.pageIndex,
      showSizeChanger: true,
      hideOnSinglePage: true,
      ...props.pagination,
    }
  }, [params.pageIndex, params.pageSize, props.pagination])

  let fetchData = useRef(async (newParams: Partial<P>) => {
    if (loading) return
    setLoading(loading)
    try {
      let dataSource: T[] = []
      let total = 0
      if (!props.api) return
      let finalParams = { ...params, ...newParams }
      let searchData = toSearchData(finalParams, props.columns)
      let apiResult = await props.api(searchData)

      dataSource = apiResult.items
      total = apiResult.total

      dataSource.forEach(
        (item: any, index: number) =>
          (item._index = (params.pageIndex! - 1) * params.pageSize! + index + 1),
      )

      if (props.useTreeTable) {
        //@ts-ignore
        dataSource = toTreeData(dataSource)
      }

      setDataSource(dataSource)
      setParams(finalParams)
    } catch (ex) {
      console.warn('fetch error', ex)
    } finally {
      setLoading(false)
    }
  })
  const onTableChange = useCallback((page: TablePaginationConfig, filters: any, sorter: any) => {
    let pageParam: PageRequest = { pageSize: page.pageSize, pageIndex: page.current }
    if (sorter) {
      if (sorter.order) {
        pageParam.orderBy = `${sorter.field} ${sorter.order === 'descend' ? 'desc' : 'asc'}`
      } else {
        pageParam.orderBy = undefined
      }
    }
    setParams((p) => ({ ...p, ...pageParam }))
  }, [])

  useEffect(() => {
    let propParams = { ...props.params }

    for (let key of Object.keys(propParams)) {
      if (key in params) {
        //@ts-ignore
        if (propParams[key] !== params[key]) {
          fetchData.current(propParams)
          return
        }
      } else {
        fetchData.current(propParams)
        return
      }
    }
  }, [props.params, params])

  const columns = useMemo(() => {
    let columns = translateColumns(props.columns)
    columns.forEach((col) => {
      if (col.dataIndex && !col.title) {
        col.title = props.titles?.[col.dataIndex.toString()]
      }
      if (!col.title) {
        console.warn('列没有设置标题:', col)
      }
      col.actions?.map((a) => (a.title = a.title ?? props.titles?.[a.code!]))
    })
    return columns
  }, [props.columns, props.titles])

  const tableContext: TableContext<T, P> = {
    props,
    dataSource,
    setDataSource,
    columns,
    params,
    setParams,
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
            pagination={params.pageSize! > 0 && pageOptions}
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
