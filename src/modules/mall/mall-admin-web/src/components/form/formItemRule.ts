import { Rule, RuleObject } from 'antd/es/form'
import { FormItemProps } from './withFormItem'
type RuleType = RuleObject['type']
const defaultValidateFields = ['required', 'min', 'max', 'pattern']

export function getRules(props: FormItemProps, validateFields?: string[]): Rule[] {
  var fields = validateFields || defaultValidateFields
  var rules: Rule[] = []
  for (let ruleName of fields) {
    if (!props.hasOwnProperty(ruleName)) continue
    let requireMsg = `${props.placeholder || '请输入'}${props.label || props.placeholder || ''}`
    if (ruleName === 'required') {
      rules.push({ required: true, message: requireMsg })
    }
    if (ruleName === 'min') {
      let min = Number(props[ruleName])
      let type: RuleType = typeof props[ruleName] == 'number' ? 'number' : 'string'
      rules.push({ min, type, message: `${props.label}必须大于等于${min}` })
    }
    if (ruleName === 'max') {
      let max = Number(props[ruleName])
      let type: RuleType = typeof props[ruleName] == 'number' ? 'number' : 'string'
      rules.push({ max, type, message: `${props.label}必须小于等于${max}` })
    }
    if (ruleName === 'pattern') {
      let pattern = new RegExp(props[ruleName]!)
      rules.push({ pattern, message: `请输入正确的${props.label}` })
    }
  }
  return rules
}
