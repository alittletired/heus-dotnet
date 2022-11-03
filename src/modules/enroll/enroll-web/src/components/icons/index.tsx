import {
  SearchOutlined,
  PlusOutlined,
  SettingOutlined,
  LoadingOutlined,
  DownloadOutlined,
  EditOutlined,
  UploadOutlined,
  DeleteOutlined,
  PlayCircleOutlined,
  DownOutlined,
  UpOutlined,
} from '@ant-design/icons'
export const icons = {
  plus: PlusOutlined,
  search: SearchOutlined,
  setting: SettingOutlined,
  loading: LoadingOutlined,
  create: PlusOutlined,
  download: DownloadOutlined,
  upload: UploadOutlined,
  edit: EditOutlined,
  delete: DeleteOutlined,
  play: PlayCircleOutlined,
}
export {
  SearchOutlined,
  PlusOutlined,
  SettingOutlined,
  LoadingOutlined,
  DownloadOutlined,
  EditOutlined,
  UploadOutlined,
  DeleteOutlined,
  PlayCircleOutlined,
  DownOutlined,
  UpOutlined,
}
export type IconKey = keyof typeof icons
export function GetIconDom(icon?: IconKey) {
  if (!icon) return undefined
  const IconComponent = icons[icon]
  return <IconComponent />
}
export default icons
