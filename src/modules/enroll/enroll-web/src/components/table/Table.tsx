import React, { useState, useEffect, useCallback, useMemo, useRef } from 'react'
import { Table, Card, TablePaginationConfig } from 'antd'
import { toSearchData, translateColumns } from './tableUtils'
import { TableInstance, TableProps } from './interface'
import SearchForm from './SearchForm'
import ToolBar from './ToolBar'
// import './index.css'
import { normalizeOptions, NameOption, OptionType, isApiOptions } from '../select'
import { toTreeData, TreeEntity } from '../tree/treeUtils'

export const TableContext = React.createContext({} as TableInstance)
export const useTable = () => React.useContext(TableContext)
export default function ApiTable<T extends object, P extends PageRequest>(props: TableProps<T, P>) {
  let [dataSource, setDataSource] = useState([] as T[])
  let queryDataRef = useRef({
    ...props.data,
    pageIndex: 1,
    pageSize: props.pagination === false ? -1 : props.pagination?.pageSize || 10,
  })

  let pageRef = useRef<TablePaginationConfig>({
    pageSize: queryDataRef.current.pageSize,
    current: queryDataRef.current.pageIndex,
    showSizeChanger: true,
    hideOnSinglePage: true,
    ...props.pagination,
  })
  let loadRef = useRef(false)
  let [loading, setLoading] = useState(false)
  let fetchData = useRef(async (param?: Partial<P>) => {
    if (loadRef.current) return
    queryDataRef.current = { ...queryDataRef.current, ...param }

    if (props.onApiBefore) {
      let canInvoke = await props.onApiBefore(queryDataRef.current)
      if (!canInvoke) {
        return
      }
    }
    loadRef.current = true
    setLoading(loadRef.current)
    try {
      let dataSource: T[] = []
      let total = 0
      if (!props.api) return
      let searchData = toSearchData(queryDataRef.current, props.columns)
      let apiResult = await props.api(searchData)

      dataSource = apiResult.items
      total = apiResult.total

      dataSource.forEach(
        (item: any, index: number) =>
          (item._index =
            (queryDataRef.current.pageIndex - 1) * queryDataRef.current.pageSize + index + 1),
      )

      if (props.useTreeTable) {
        pageRef.current.pageSize = dataSource.length
        //@ts-ignore
        dataSource = toTreeData(dataSource)
      }
      pageRef.current.total = total
      pageRef.current.current = queryDataRef.current.pageIndex
      props.OnApiSuccess?.(apiResult)
      setDataSource(dataSource)
    } catch (ex) {
      // console.warn(ex)
    } finally {
      loadRef.current = false
      setLoading(loadRef.current)
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
    fetchData.current(pageParam as any)
  }, [])
  const search = useRef((data?: any) => {
    queryDataRef.current = {
      ...props.data,
      pageIndex: 1,
      pageSize: props.pagination === false ? -1 : props.pagination?.pageSize || 10,
    }

    return fetchData.current(data)
  })
  useEffect(() => {
    let data = { ...props.data }

    for (let key of Object.keys(data)) {
      if (key in queryDataRef.current) {
        //@ts-ignore
        if (data[key] !== queryDataRef.current[key]) {
          search.current(data)
          return
        }
      } else {
        search.current(data)
        return
      }
    }
  }, [props.data])
  const apiOptions = useRef<Record<string, Promise<OptionType[]>>>({})
  const [options, setOptions] = useState(() => {
    let options: Record<string, NameOption[]> = {}
    for (let col of props.columns) {
      if (col.dataIndex && col.options) {
        let api = col.options
        if (isApiOptions(api)) {
          apiOptions.current[col.dataIndex.toString()] = api(col.optionsParam)
        } else {
          options[col.dataIndex.toString()] = normalizeOptions(api)
        }
      }
    }
    return options
  })
  useEffect(() => {
    Promise.all(Object.values(apiOptions.current)).then((res) => {
      setOptions((options) => {
        let keys = Object.keys(apiOptions.current)
        for (let idx = 0; idx < keys.length; idx++) {
          options[keys[idx]] = normalizeOptions(res[idx])
        }
        return options
      })
    })
  }, [])
  const columns = useMemo(() => {
    let columns = translateColumns(props.columns)
    columns.forEach((col) => {
      col.title = col.title ?? props.titles?.[col.dataIndex?.toString()]
      if (!col.title) {
        console.warn('列没有设置标题:', col)
      }
      col.actions?.map((a) => (a.title = a.title ?? props.titles?.[a.code!]))
    })
    return columns
  }, [props.columns, props.titles])

  const table: TableInstance<T> = {
    props,
    dataSource,
    setDataSource,
    columns,
    options,
    data: queryDataRef.current,
    search: search.current,
    reset: search.current,
    reload: fetchData.current,
  }

  if (props.table) {
    Object.assign(props.table, table)
  }
  useEffect(() => {
    search.current()
  }, [])

  props.toolBar?.forEach((op) => (op.title = op.title ?? props.title?.[op.code!]))
  return (
    <TableContext.Provider value={table}>
      <div className="api-table">
        <SearchForm />
        <Card
          bodyStyle={{ height: '100%', padding: 0 }}
          title={props.tableTitle}
          extra={<ToolBar items={props.toolBar} />}>
          <Table<T>
            scroll={{ x: 'max-content' }}
            key={dataSource.length}
            rowKey={props.rowKey ?? 'id'}
            size="small"
            expandable={{ defaultExpandAllRows: true }}
            {...props}
            loading={loading}
            onChange={onTableChange}
            dataSource={dataSource}
            pagination={pageRef.current.pageSize > 0 && pageRef.current}
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
