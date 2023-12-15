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

var loaderjsLoaded = false;

const Monaco = {

    _monaco: null,

    Init: async function Init(id, interop, options) {

        await LoadScript('_content/Biwen.Blazor.Components/monaco-editor/min/vs/loader.js');
        require.config({ paths: { vs: './_content/Biwen.Blazor.Components/monaco-editor/min/vs' } });

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

            this._monaco = editor;
            this._monaco.layout();
            interop.invokeMethodAsync('OnEditorLoad');

        });
    },

    SetOptions: function (options) {

        this._monaco.setValue(options.value);
        this._monaco.updateOptions({
            language: options.language,
            theme: options.theme
        });

        monaco.editor.setModelLanguage(monaco.editor.getModels()[0], options.language);
    },

    GetVal: function () {
        return this._monaco.getValue();
    },

    SetVal: function (val) {
        this._monaco.setValue(val);
    },

    Layout: function () {
        this._monaco.layout();

    },

    Dispose: function () {
        if (this._monaco) {
            this._monaco.dispose();
            this._monaco = null;
        }
    }
};






export { Monaco };