import ActionButton, { ActionButtonProps } from './ActionButton'
import ExportButton, { ExportProps } from './ActionExportButton'
import ImportButton, { ImportExeclProps } from './ActionImportButton'
type ExportButtonProps<T> = { actionType: 'export' } & ExportProps<T>
type ImportButtonProps<T> = { actionType: 'import' } & ImportExeclProps<T>
export type ActionComponentProps<T> = ActionButtonProps<T> &
  (
    | ExportButtonProps<T>
    | ImportButtonProps<T>
    | {
        actionType?: 'create'
      }
  )

export default function ActionComponent<T>(props: ActionComponentProps<T>) {
  let { actionType, ...restProps } = props
  if (actionType === 'export') return <ExportButton {...restProps} />
  if (actionType === 'import') return <ImportButton {...(restProps as ImportButtonProps<T>)} />
  return <ActionButton {...restProps} />
}
