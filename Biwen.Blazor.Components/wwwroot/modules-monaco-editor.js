/// <reference path="monaco-editor/monaco.d.ts" />

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

const editors = [];

var loaderjsLoaded = false;

async function Init(id, interop, options) {

    if (!loaderjsLoaded) {
        await LoadScript('_content/Biwen.Blazor.Components/monaco-editor/min/vs/loader.js');
        require.config({ paths: { vs: './_content/Biwen.Blazor.Components/monaco-editor/min/vs' } });
        loaderjsLoaded = true;
    }

    require(['vs/editor/editor.main'], function () {

        let container = document.getElementById(id);
        const editor = monaco.editor.create(container, {
            ariaLabel: 'Code Editor',
            value: options.value,
            language: options.language,
            theme: options.theme,
            automaticLayout: true,
            readOnly: options.readOnly,
            minimap: {
                enabled: false
            },
        });

        monaco.editor.setModelLanguage(monaco.editor.getModels()[0], options.language);
        editor.onDidChangeModelContent(function (e) {
            interop.invokeMethodAsync('UpdateValueAsync', editor.getValue());
        });

        editor.layout();

        editors[id] = editor;

        interop.invokeMethodAsync('OnEditorLoad');

    });
}

function SetOptions(id, options) {

    if (!editors[id])
        return;

    editors[id].setValue(options.value);

    editors[id].updateOptions({
        language: options.language,
        theme: options.theme
    });

    monaco.editor.setModelLanguage(monaco.editor.getModels()[0], options.language);
}
function GetVal(id) {
    if (!editors[id])
        return '';
    return editors[id].getValue();
}
function SetVal(id, val) {
    if (!editors[id])
        return;
    editors[id].setValue(val);
}

function Layout(id) {
    if (!editors[id])
        editors[id].layout();
}

function Dispose(id) {
    if (!editors[id])
        return;
    editors[id].dispose();
    editors[id] = null;
}





export { Init, GetVal, SetVal, SetOptions, Layout, Dispose };