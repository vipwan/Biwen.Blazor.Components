import "./prism.js";

//加载css文件
export function loadStyle(resourceUrl) {
    return new Promise((resolve, reject) => {
        const link = document.createElement('link');
        link.rel = 'stylesheet';
        link.onload = () => resolve();
        link.onerror = () => reject();
        link.href = resourceUrl;
        document.head.appendChild(link);
    });
}

export function highlight(ele, flag, o) {
    Prism.highlightAllUnder(ele, flag, o);
}