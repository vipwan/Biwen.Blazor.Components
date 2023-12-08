/// <reference path="./easymde/types/easymde.d.ts" />

//doc:
//https://github.com/Ionaru/easy-markdown-editor#options-example


import './easymde/dist/easymde.min.js';

const editors = [];

const Editor = {

    icons: ["code", "table"],

    init: function (elementId, options) {

        if (options.uploadImage) {
            this.icons.push("upload-image");
        }

        const editor = new EasyMDE({
            element: document.getElementById(elementId),
            showIcons: this.icons,
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
            imagePathAbsolute: true,//使用绝对路径
            //imageUploadFunction: options.imageUploadFunction,
        });

        editors[elementId] = editor;
    }
}

function GetVal(elementId) {
    return editors[elementId].value();
}
function SetVal(elementId, val) {
    editors[elementId].value(val);
}

export { Editor, GetVal, SetVal };
