import {FormItemProps} from './withFormItem'
import {Rule, RuleObject} from 'antd/lib/form'
type RuleType = RuleObject['type']
const allValidateFields = ['required', 'min', 'max', 'pattern']

export function getRules(props: FormItemProps, validateFields?: string[]): Rule[] {
  validateFields = validateFields || allValidateFields
  return validateFields
    .filter((ruleName) => props.hasOwnProperty(ruleName))
    .map((ruleName) => {
      let requireMsg = `${props.placeholder || '请输入'}${
        props.label || props.placeholder || ''
      }`
      if (ruleName === 'required') {
        return {required: true, message: requireMsg}
      }
      if (ruleName === 'min') {
        let min = Number(props[ruleName])
        let type: RuleType = typeof props[ruleName] == 'number' ? 'number' : 'string'
        return {min, type, message: `${props.label}必须大于等于${min}`}
      }
      if (ruleName === 'max') {
        let max = Number(props[ruleName])
        let type: RuleType = typeof props[ruleName] == 'number' ? 'number' : 'string'
        return {max, type, message: `${props.label}必须小于等于${max}`}
      }
      if (ruleName === 'pattern') {
        let pattern = new RegExp(props[ruleName])
        return {pattern, message: `请输入正确的${props.label}`}
      }
      return null
    })
}
