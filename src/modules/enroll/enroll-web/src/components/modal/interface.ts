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
interface FormModalProps {
  title?: string
  width?: string | number
  viewType?: ViewType
  overlayType?: OverlayType
}
export type ModalComponentProps<M, P = {}> = { model?: M } & P & {
    setModalProps?: (props: FormModalProps) => void
  }

export interface ModalComponent<M, P = {}> extends React.FC<ModalComponentProps<M, P>> {
  defaultModalProps?: (props: ModalComponentProps<M, P>) => FormModalProps
}
