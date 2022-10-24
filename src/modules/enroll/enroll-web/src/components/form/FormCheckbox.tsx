import React, {useMemo} from 'react'
import {Checkbox} from 'antd'
import {CheckboxProps} from 'antd/lib/checkbox'
import withFormItem from './withFormItem'
const FormCheckbox = withFormItem(Checkbox)
FormCheckbox.defaulItemProps = {
  valuePropName: 'checked',
  noStyle: true,
  placeholder: '请选择',
}
export default FormCheckbox
