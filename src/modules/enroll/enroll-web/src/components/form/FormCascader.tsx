import React from 'react'
import { Cascader, CascaderProps } from 'antd'
import withFormItem from './withFormItem'
import { OptionType } from '../select/OptionUtils'
export const idNameFieldNames = { label: 'name', value: 'id', children: 'children' }
export const titleFieldNames = { label: 'title', value: 'value', children: 'children' }
export const labelFieldNames = { label: 'label', value: 'value', children: 'children' }

const FormCascader = withFormItem((props: CascaderProps<OptionType>) => {
  var fieldNames = labelFieldNames
  return <Cascader fieldNames={fieldNames} {...props} />
})
FormCascader.defaultProps = { placeholder: '请选择' }
export default FormCascader

// export const FormCityCascader: React.FC<CityProps> = (props) => {
//   let {onlyCity, ...rest} = props
//   // 延时计算city的数据
//   if (onlyCity && cityData.length === 0) {
//     cityData = mapCity.map((p) => {
//       let children = (p.children as any)?.map((c: any) => {
//         return {label: c.label, value: c.value}
//       })
//       return {...p, children}
//     })
//   }
//   let options = onlyCity ? cityData : mapCity
//   return <FormCascader {...rest} options={options} />
// }
