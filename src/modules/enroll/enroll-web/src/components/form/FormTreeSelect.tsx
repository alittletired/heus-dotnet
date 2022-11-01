import { TreeSelect, TreeSelectProps } from 'antd'
import withFormItem from './withFormItem'
export type FormItemTreeSelect = TreeSelectProps & { control: 'treeSelect ' }
const FormTreeSelect = withFormItem(TreeSelect)
export default FormTreeSelect
