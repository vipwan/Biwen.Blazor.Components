/// <reference path="./easymde/types/easymde.d.ts" />

//doc:
//https://github.com/Ionaru/easy-markdown-editor#options-example

import './easymde/dist/easymde.min.js';


const Editor = {


    icons: ["code", "link", "table"],

    _editor: null,
    // 初始化
    Init: function (elementId, options) {

        // 使用拖拽上传图片.
        if (options.uploadImage) {
            this.icons.push("upload-image");
        }

        let editor = new EasyMDE({
            element: document.getElementById(elementId),
            showIcons: this.icons,
            hideIcons: ["guide"],
            autosave: {
                //enabled: true,
                //uniqueId: elementId,
            },
            //lineNumbers: true,
            spellChecker: false,
            uploadImage: options.uploadImage,
            imageUploadEndpoint: options.imageUploadEndpoint,
            imageMaxSize: options.imageMaxSize,
            imageAccept: options.imageAccept,
            promptURLs: true,
            imagePathAbsolute: true,//使用绝对路径
            //imageUploadFunction: options.imageUploadFunction,
        });

        this._editor = editor;
    },
    // 获取值
    GetVal: function () {
        return this._editor.value();
    },
    // 设置值
    SetVal: function (val) {
        this._editor.value(val);
    },
    // 释放内存
    Dispose: function () {
        this._editor.cleanup();
        this._editor = null;
    }
}

export { Editor };
