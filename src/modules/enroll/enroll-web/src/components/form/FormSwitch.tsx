import React from 'react'
import {Switch} from 'antd'
import withFormItem from './withFormItem'

const FormSwitch = withFormItem(Switch)
FormSwitch.defaulItemProps = {valuePropName: 'checked', placeholder: '请选择'}
export default FormSwitch
