import React, {
  ComponentClass,
  PropsWithChildren,
  ReactElement,
  ComponentType,
  ForwardRefExoticComponent,
} from 'react'
import {Form, Input} from 'antd'
import {FormItemProps as AntFormItemProps} from 'antd/lib/form'
import {getRules} from './formItemRule'
import FormContext from './FormContext'
const AntFormItem = Form.Item

export interface FormItemProps {
  /**是否必填 */
  required?: boolean
  /**标题 */
  label?: string
  placeholder?: string
  /**是否隐藏 */
  hidden?: boolean
  /**是否显示冒号 */
  colon?: boolean
  name?: string
  pattern?: string
  //string 类型为字符串最大长度；number 类型时为最大值；array 类型时为数组最大长度
  max?: number | string
  //string 类型为字符串最大长度；number 类型时为最大值；array 类型时为数组最大长度
  min?: number | string
  noStyle?: boolean
  extra?: React.ReactNode
}
interface WithComponent<P> {
  <D>(props: PropsWithChildren<P & FormItemProps>): JSX.Element
  defaulItemProps?: Partial<AntFormItemProps & FormItemProps>
}
function getProps<P>(props: P & AntFormItemProps & FormItemProps): [AntFormItemProps, P] {
  let {
    required,
    label,
    placeholder,
    colon,
    name,
    pattern,
    max,
    hidden,
    valuePropName,
    min,
    extra,
    noStyle,
    ...rest
  } = props
  // (rest as any).placeholder = rest.placeholder ?? placeholder)
  let rules = getRules(props)
  return [
    {rules, label, noStyle, colon, valuePropName, name, extra},
    {placeholder: placeholder, ...rest} as P,
  ]
}
export default function withFormItem<P>(Component: ComponentType<P>): WithComponent<P> {
  const WithFormItem: WithComponent<P> = (props) => {
    const formContext = React.useContext(FormContext)

    if (props.hidden) return null
    let label = props.label ?? formContext.titles?.[props.name]
    let [itemProps, restProps] = getProps({
      ...WithFormItem.defaulItemProps,
      ...props,
      label,
    })
    //消除formitem的警告
    if (Component.length === 2) {
      Component = React.forwardRef(Component as any) as unknown as ComponentType<P>
    }

    return (
      <AntFormItem {...itemProps} label={formContext.noLabel ? '' : label}>
        <Component {...restProps} />
      </AntFormItem>
    )
  }
  return WithFormItem
}
