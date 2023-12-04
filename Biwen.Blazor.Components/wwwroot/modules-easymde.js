/// <reference path="./easymde/types/easymde.d.ts" />

//ES6 规范：export default 和 import 配对
//CommonJS 规范：module.exports 和 require 配对

import './easymde/dist/easymde.min.js';

const editors = [];

const Editor = {
    init: function (elementId) {
        const editor = new EasyMDE({
            element: document.getElementById(elementId),
            showIcons: ["code", "table", "upload-image"],
            autosave: {
                //enabled: true,
                //uniqueId: elementId,
            },
            //lineNumbers: true,
            spellChecker: false,
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
