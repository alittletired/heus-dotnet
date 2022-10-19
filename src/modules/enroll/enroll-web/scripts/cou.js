const axios = require('axios')
const path = require('path')
const qs = require('qs')
const fs = require('fs')

const writeJson = (data, pathName) => {
  const dir = path.join(process.cwd(), '/scripts/data')
  fs.writeFileSync(path.join(dir, `${pathName}.json`), JSON.stringify(data), 'utf8')
}

const getData = (pathName) => {
  const dir = path.join(process.cwd(), '/scripts/data')
  let rawData = fs.readFileSync(path.join(dir, `${pathName}.json`))
  let data = JSON.parse(rawData)
  return data
}

const wordCouData = {}
const childNames = [
  '高频词汇',
  '第一册',
  '一年级',
  '一年级上',
  '一年级下',
  '二年级',
  '二年级上',
  '二年级下',
  '三年级',
  '三年级上',
  '三年级下',
  '四年级',
  '四年级上',
  '四年级下',
]
const nameMap = {
  高频词汇: '幼儿',
  第一册: '一年级',
  一年级: '一年级',
  一年级上: '一年级',
  一年级下: '一年级',
  二年级: '二年级',
  二年级上: '二年级',
  二年级下: '二年级',
  三年级: '三年级',
  三年级上: '三年级',
  三年级下: '三年级',
  四年级: '四年级',
  四年级上: '四年级',
  四年级下: '四年级',
}

//移除 '单词'

const processWordCo = () => {
  const list = getData('单词课件')
  list.forEach((item) => {
    if (item.dname.indexOf('肯登攀') == -1) {
      if (!wordCouData[item.version]) {
        wordCouData[item.version] = {}
      }

      const childCouData = wordCouData[item.version]
      const name = item.dname.replace('单词', '')
      let childName = ''

      childNames.forEach((itemName) => {
        if (name.indexOf(itemName) > -1) {
          childName = nameMap[itemName]
        }
      })

      if (childName) {
        if (!childCouData[childName]) {
          childCouData[childName] = []
        }
        // item.dname = item.dname.split('-')[1].replace('-', 0)
        const items = childCouData[childName]
        items.push(item)
      }
    }
  })

  writeJson(wordCouData, 'wordCou')
}

processWordCo()
