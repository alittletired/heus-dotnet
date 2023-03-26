import React, { PropsWithChildren, Ref } from 'react'
import withFormItem from './withFormItem'

const Text: React.FC<PropsWithChildren> = (props) => <div>{props.children}</div>

// Text.defaultProps = { className: 'form-item-text' }
export default Text
