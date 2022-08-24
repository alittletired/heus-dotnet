import path from 'path'
import fs from 'fs'
import prettier from 'prettier'
const options = prettier.resolveConfig.sync(
    path.join(process.cwd(), 'prettier.config.js')
)
const srcDir = path.join(process.cwd(), 'src')
const audiosDir = path.join(process.cwd(), 'public', 'audios')
var files = fs.readdirSync(audiosDir)
var audioExtensions = ['.mp3', '.wav', '.m4a']
let audios: Record<string, string> = {}
for (let file of files) {
    if (audioExtensions.some((ext) => file.endsWith(ext))) {
        audios[file.substr(0, file.lastIndexOf('.'))] = file
    }
}

console.warn(audios)

let content = prettier.format('export default ' + JSON.stringify(audios), {
    ...options,
    parser: 'typescript',
})
fs.writeFileSync(path.join(srcDir, `audios.ts`), content, 'utf8')
