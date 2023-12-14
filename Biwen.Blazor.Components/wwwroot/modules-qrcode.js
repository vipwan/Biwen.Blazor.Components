//import * as qrcodeMin from './qrcode-generator/qrcode.min.js';
//import QRCode from './qrcode-generator/qrcode.min.js';

// 加载脚本
const LoadScript = content => {

    const normalizeLink = link => {
        let url = link
        if (url.indexOf('./') === 0) {
            url = url.substring(2)
        }
        while (url.indexOf('../') === 0) {

            url = url.substring(3)
        }
        return url
    }

    const scripts = [...document.getElementsByTagName('script')]
    const url = normalizeLink(content)
    let link = scripts.filter(function (link) {
        return link.src.indexOf(url) > -1
    })
    if (link.length === 0) {
        const script = document.createElement('script')
        link.push(script)
        script.setAttribute('src', content)
        document.body.appendChild(script)
        script.onload = () => {
            script.setAttribute('loaded', true)
        }
    }
    return new Promise((resolve, reject) => {
        const handler = setInterval(() => {
            const done = link[0].getAttribute('loaded') === 'true'
            if (done) {
                clearInterval(handler)
                resolve()
            }
        }, 20)
    })
}

var qrcodeLoaded = false;
async function InitQRcode(element, typeNumber, level, content, cellSize) {

    if (!qrcodeLoaded) {
        await LoadScript('_content/Biwen.Blazor.Components/qrcode/qrcode.min.js');
        qrcodeLoaded = true;
    }

    var qr = qrcode(typeNumber, level);
    qr.addData(content);
    qr.make();
    document.getElementById(element).innerHTML = qr.createImgTag(cellSize);

}

export { InitQRcode };