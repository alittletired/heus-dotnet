import ActionButton, {ActionButtonProps} from './ActionButton'

export default function ActionAnchor<T>(props: ActionButtonProps<T>) {
  return <ActionButton type="link" className="action-anchor" {...props} />
}
