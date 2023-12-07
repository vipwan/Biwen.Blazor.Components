/// <reference path="./easymde/types/easymde.d.ts" />

//doc:
//https://github.com/Ionaru/easy-markdown-editor#options-example


import './easymde/dist/easymde.min.js';

const editors = [];

const Editor = {
    init: function (elementId, options) {
        const editor = new EasyMDE({
            element: document.getElementById(elementId),
            showIcons: ["code", "table", "upload-image"],
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
