import ExcelJS from 'exceljs'
import { formatDate } from './dateUtils'
import { getOptionTitle, getOptionValue, normalizeOptions, OptionType } from '@/components/select'
import { ColumnProps } from '@/components/table/interface'
import { saveAs } from './fileUtils'
const Cells = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'
const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8'
interface SheetOption {
  columns: ColumnProps[]
  sheetName: string
  data: any[]
}
interface ExportOption {
  fileName?: string
  sheets: SheetOption[]
}
interface OptionValidation {
  colIndex: number
  colKey: string
  options: OptionType[]
  validation: ExcelJS.DataValidation
}
interface ImportOption {
  file: Blob
  importApi: any
  columns: ColumnProps[]
}
function covert2ExeclColumns(columns: ColumnProps[]): [ExcelJS.Column[], OptionValidation[]] {
  let optionValidations: OptionValidation[] = []
  let sheetCols = columns
    .filter((col) => col.dataIndex)
    .map((col, colIndex) => {
      let sheetCol = { header: col.title, key: col.dataIndex, hidden: false }
      if (col.options) {
        sheetCol.key = `${String(col.dataIndex)}_option`
        let options = normalizeOptions(col.options as any)
        let formulae = options.map((option) => option.label).join(',')

        optionValidations.push({
          colIndex,
          colKey: sheetCol.key,
          options,
          validation: {
            type: 'list',
            allowBlank: false,
            formulae: ['"' + formulae + '"'],
          },
        })
      }

      return sheetCol
    })
  sheetCols.push({ header: 'Id', key: 'id', hidden: true })
  return [sheetCols as ExcelJS.Column[], optionValidations]
}
function fillSheet(worksheet: ExcelJS.Worksheet, sheetOption: SheetOption) {
  let { columns, data } = sheetOption
  let [sheetCols, optionValidations] = covert2ExeclColumns(columns)
  worksheet.columns = sheetCols
  data.forEach((item, index) => {
    item._index = index + 1
    for (let optionValidation of optionValidations) {
      let { colIndex, colKey, options } = optionValidation
      let value = item[colKey.split('_')[0]]
      item[colKey] = getOptionTitle(value, options)
    }
    worksheet.addRow(item)
  })
  data.forEach((_, index) => {
    for (let optionValidation of optionValidations) {
      let { colIndex, validation } = optionValidation
      worksheet.getCell(Cells[colIndex] + (index + 2)).dataValidation = validation
    }
  })
}

export async function exportExcel<T>(option: ExportOption) {
  const workbook = new ExcelJS.Workbook()
  workbook.properties.date1904 = true
  let { sheets, fileName } = option
  for (let sheetOption of option.sheets) {
    const sheet = workbook.addWorksheet(sheetOption.sheetName)
    fillSheet(sheet, sheetOption)
  }

  if (!fileName) {
    fileName = `${sheets[0].sheetName}-${formatDate(new Date().getTime())}.xlsx`
  }
  let data = await workbook.xlsx.writeBuffer()
  const blob = new Blob([data], { type: EXCEL_TYPE })
  saveAs(blob, fileName)
}
function checkTableHeader(row: ExcelJS.Row, columns: ExcelJS.Column[]) {
  row.eachCell((cell, colNumber) => {
    if (cell.value !== columns[colNumber - 1].header) throw new Error('表格格式不对')
  })
}
export async function importExcel(option: ImportOption) {
  let { file, columns, importApi } = option
  const workbook = new ExcelJS.Workbook()
  let arrayBuffer = await file.arrayBuffer()
  await workbook.xlsx.load(arrayBuffer)
  let sheet = workbook.getWorksheet(1)
  let [sheetCols, optionValidations] = covert2ExeclColumns(columns)
  var data: any[] = []
  sheet.eachRow((row, rowNumber) => {
    if (rowNumber === 1) {
      checkTableHeader(row, sheetCols)
    } else {
      var item: Record<string, any> = {}
      row.eachCell((cell, colNumber) => {
        let { key } = sheetCols[colNumber - 1]
        if (key) {
          item[key] = cell.value
        }
      })
      for (let dataValidation of optionValidations) {
        let { options, colKey } = dataValidation
        item[colKey.replace('_option', '')] = getOptionValue(options, item[colKey])
      }
      data.push(item)
    }
  })
  await importApi(data)
}
