import React, { PropsWithChildren, Ref } from 'react'
import withFormItem from './withFormItem'

const FormText: React.FC<PropsWithChildren> = (props) => <div>{props.children}</div>
const FormItemText = withFormItem(FormText)
FormItemText.defaultProps = { className: 'form-item-text' }
export default FormItemText
