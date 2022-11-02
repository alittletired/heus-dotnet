import React, { PropsWithChildren, ComponentType } from 'react'
import { Form, FormItemProps as AntFormItemProps } from 'antd'
import { getRules } from './formItemRule'
import FormContext from './FormContext'
const AntFormItem = Form.Item

export interface FormItemProps extends AntFormItemProps {
  pattern?: string
  //string 类型为字符串最大长度；number 类型时为最大值；array 类型时为数组最大长度
  max?: number | string
  //string 类型为字符串最大长度；number 类型时为最大值；array 类型时为数组最大长度
  min?: number | string
}
type WithComponent<P> = React.FC<P & FormItemProps> & {
  defaulItemProps?: Partial<FormItemProps>
  defaultControlProps?: Partial<P>
}

function extractProps<P>(props: P & FormItemProps): [AntFormItemProps, P] {
  let {
    required,
    label,
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
  return [{ rules, label, noStyle, colon, valuePropName, name, extra }, { ...rest } as P]
}

export default function withFormItem<P>(Component: ComponentType<P>) {
  const WithFormItem: WithComponent<P> = (props) => {
    const formContext = React.useContext(FormContext)
    if (props.hidden) return null
    let finalProps = {
      ...WithFormItem.defaulItemProps,
      ...props,
    }

    if (formContext.noLabel) {
      finalProps.label = ''
    } else if (!props.label && formContext.labels && props.name) {
      finalProps.label = formContext.labels?.[props.name.toString()]
    }
    let [itemProps, controlProps] = extractProps(finalProps)
    WithFormItem.defaulItemProps = itemProps
    WithFormItem.defaultControlProps = controlProps

    // //消除formitem的警告
    // if (Component.length === 2) {
    //   Component = React.forwardRef(Component as any) as unknown as ComponentType<P>
    // }

    return (
      <AntFormItem {...itemProps}>
        <Component {...controlProps} />
      </AntFormItem>
    )
  }
  return WithFormItem
}
