import React, {useMemo} from 'react'
import {Cascader} from 'antd'
import {CascaderProps} from 'antd/lib/cascader'
import withFormItem from './withFormItem'
const fieldNames = {label: 'name', value: 'id', children: 'children'}
const FormCascader = withFormItem((props: CascaderProps<any>, ref: any) => {
  console.warn('options', props.options)

  return <Cascader ref={ref} fieldNames={fieldNames} {...props} />
})
FormCascader.defaulItemProps = {placeholder: '请选择'}
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