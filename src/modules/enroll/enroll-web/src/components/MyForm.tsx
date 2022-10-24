import React from 'react'
import {FormItemProps} from './form/withFormItem'
export enum ControlType {
  /** check */
  CHECK = 5,
  /** input */
  INPUT = 0,
  /** multiSelect */
  MULTI_SELECT = 2,
  /** radio */
  RADIO = 4,
  /** select */
  SELECT = 1,
  /** treeSelect */
  TREE_SELECT = 3,
}
interface MyFormItemOption extends FormItemProps {
  controlType: ControlType
  optionGroup?: string
}
interface MyForm {
  items: ControlType[]
}
