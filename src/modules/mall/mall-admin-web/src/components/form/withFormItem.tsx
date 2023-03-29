import React, { PropsWithChildren, ComponentType } from 'react'
import { Form, FormItemProps as AntFormItemProps } from 'antd'
import { getRules } from './formItemRule'
import { usePageContext } from '../../views/PageContext'
import { useFormContext } from './FormContext'
const AntFormItem = Form.Item
const Empty: React.ForwardRefRenderFunction<any, any> = (props, ref) => {
  return <></>
}
const EmptyRef = React.forwardRef(Empty)
const REACT_FORWARD_REF_TYPE = EmptyRef.$$typeof
function isForwardRefComponent<P>(
  Component: React.ComponentType<P>,
): Component is React.ForwardRefExoticComponent<P> {
  return (Component as React.ForwardRefExoticComponent<P>).$$typeof == REACT_FORWARD_REF_TYPE
}

export interface FormItemProps extends AntFormItemProps {
  pattern?: string
  //string 类型为字符串最大长度；number 类型时为最大值；array 类型时为数组最大长度
  max?: number | string
  //string 类型为字符串最大长度；number 类型时为最大值；array 类型时为数组最大长度
  min?: number | string
}
type WithComponent<P> = React.FC<P & FormItemProps> & {
  defaultProps?: Partial<P & FormItemProps> | undefined
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
  return [
    {
      required,
      label,
      colon,
      name,
      hidden,
      valuePropName,
      extra,
      noStyle,
    },
    { ...rest } as P,
  ]
}

export default function withFormItem<P>(Component: ComponentType<P>) {
  const WithFormItem: WithComponent<P> = (props) => {
    const pageContext = usePageContext()
    const formContext = useFormContext()
    if (props.hidden) return null
    let finalProps = {
      ...WithFormItem.defaultProps,
      ...props,
    }

    if (formContext.noLabel) {
      finalProps.label = undefined
    } else if (!props.label && props.name) {
      finalProps.label = pageContext.labels?.[props.name.toString()] as any
    }
    let [itemProps, controlProps] = extractProps(finalProps)

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
  // if (isForwardRefComponent(Component)) {
  //   var ForwardItem = React.forwardRef(WithFormItem)
  // }
  return WithFormItem
}
