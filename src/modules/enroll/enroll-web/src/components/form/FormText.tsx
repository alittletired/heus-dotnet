import React, {Ref} from 'react'
import withFormItem from './withFormItem'

const RefFormItemText = React.forwardRef<any, {children?: React.ReactNode}>(
  (props, ref: Ref<any>) => {
    return <div ref={ref}>{props.children}</div>
  },
)
const FormItemText = withFormItem(RefFormItemText)
FormItemText.defaulItemProps = {className: 'form-item-text'}
export default FormItemText
