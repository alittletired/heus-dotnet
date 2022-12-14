// 手机号正则
const regPhone = /^1\d{10}$/
// 身份证正则
const regIdCard = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/
//  邮箱正则
const regEmail = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/
// 金额
const regMoney = /^(\d+)((?:\.\d+)?)$/
export const isPhone = (phone: string) => regPhone.test(phone)
export const isIdCard = (idCard: string) => regIdCard.test(idCard)
export const isEmail = (email: string) => regEmail.test(email)
export const isMoney = (money: string) => regMoney.test(money)
const regUrl = /(((^https?:(?:\/\/)?)(?:[-;:&=+$,\w]+@)?[A-Za-z0-9.-]+(?::\d+)?|(?:www.|[-;:&=+$,\w]+@)[A-Za-z0-9.-]+)((?:\/[+~%/.\w-_]*)?\??(?:[-+=&;%@.\w_]*)#?(?:[\w]*))?)$/

export function isUrl(path: string): boolean {
  return regUrl.test(path) || path.indexOf('/') === 0
}
