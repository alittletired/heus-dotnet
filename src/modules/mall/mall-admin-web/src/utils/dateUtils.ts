import dayjs from 'dayjs'
export function formatDateTime(date?: string, format = 'YYYY-MM-DD hh:mm:ss') {
  if (!date) return ''
  if (typeof date === 'string') return date.split('.')[0].replace('T', ' ')
  if (typeof date === 'number') return dayjs(date).format(format)
  return ''
}

export function formatDate(date?: string | number, format = 'YYYY-MM-DD') {
  if (!date) return ''
  if (typeof date === 'string') return date.split('T')[0]
  if (typeof date === 'number') return dayjs(date).format(format)
  return ''
}
