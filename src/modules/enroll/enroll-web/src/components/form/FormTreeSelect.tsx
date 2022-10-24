import React, {useMemo, useRef} from 'react'
import {TreeSelect} from 'antd'
import {TreeSelectProps} from 'antd/lib/tree-select'
import withFormItem from './withFormItem'

const FormTreeSelect = withFormItem(TreeSelect)
export default FormTreeSelect
