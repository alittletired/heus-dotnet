import React from 'react'
import { FormItem, Form, overlay } from '@/components'
import { ModalFormProps } from './interface'

function confirm<D, Path, Return, P>(props: ModalFormProps<D, Path, Return, P>) {
  const MForm: ModalComponent<D> = () => {
    return (
      <Form {...props.config} initialValues={props.config.initialValues || props.modal}>
        {props.items.map((item) => {
          const key = `${item.type}-${item.config.name}`
          switch (item.type) {
            case 'input':
              var fixDataAttr = { 'data-key': key }
              var FormInput = FormItem.Input as any
              return <FormInput key={key} {...item.config} {...fixDataAttr} />
            case 'textArea':
              return <FormItem.TextArea key={key} {...item.config} />
            case 'inputNumber':
              //@ts-ignore
              return <FormItem.InputNumber key={key} {...item.config} />
            case 'switch':
              return <FormItem.Switch key={key} {...item.config} />
            case 'date':
              return <FormItem.DatePicker key={key} {...item.config} />
            case 'text':
              return <FormItem.Text key={key} {...item.config} />
            case 'verifyCode':
              //@ts-ignore
              return <FormItem.VerifyCode key={key} {...item.config} />
            default:
              return <div key={key} />
          }
        })}
        <style jsx global>{`
          .modal {
            width: 640px;
          }
          .modal .ant-modal-body .ant-tabs {
            margin-top: -24px;
          }
          /* 上边距100，头部高度56，footer高度 53，下边距20 */
          .modal .ant-modal-body {
            max-height: calc(100vh - 230px);
            overflow-y: auto;
          }
        `}</style>
      </Form>
    )
  }
  MForm.defaultModalProps = () => ({ title: props.title })
  return overlay.showForm(MForm, props.modal)
}

const ModalForm = {
  confirm,
}

export default ModalForm
