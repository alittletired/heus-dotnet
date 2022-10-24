import ActionButton, {ActionButtonProps} from './ActionButton'
import ExportButton, {ExportProps} from './ActionExportButton'
import ImportButton, {ImportExeclProps} from './ActionImportButton'
type ExportButtonProps = {buttonType: 'export'} & ExportProps
type ImportButtonProps = {buttonType: 'import'} & ImportExeclProps
export type ActionComponentProps<T> = ActionButtonProps<T> &
  (
    | ExportButtonProps
    | ImportButtonProps
    | {
        buttonType?: 'create'
      }
  )

export default function ActionComponent<T>(props: ActionComponentProps<T>) {
  let {buttonType, ...restProps} = props
  if (buttonType === 'export') return <ExportButton {...restProps} />
  if (buttonType === 'import')
    return <ImportButton {...(restProps as ImportButtonProps)} />
  return <ActionButton {...restProps} />
}
