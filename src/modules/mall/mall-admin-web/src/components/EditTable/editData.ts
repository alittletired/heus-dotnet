import FormInput from '../form/FormInput'
import FormSelect from '../form/FormSelect'
export type EditType = 'input' | 'select'
export const editControls: {[key in EditType]: any} = {
  input: FormInput,
  select: FormSelect,
}
