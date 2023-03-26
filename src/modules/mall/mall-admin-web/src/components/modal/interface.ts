import { FormItemProps } from '../form/withFormItem'
import { FormProps } from '../form/Form'
import { OverlayType } from '../overlay'

export interface ModalFormProps<D, Path, Return, P> {
  title: string
  modal: D
  config: FormProps<D, Path, Return>
  items: FormItemConfig<P>[]
}

export interface FormItemConfig<P> {
  type: 'input' | 'textArea' | 'inputNumber' | 'switch' | 'date' | 'text' | 'verifyCode'
  config: FormItemProps & P
}
