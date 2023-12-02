// 加载css文件
function loadStyle(resourceUrl) {
    return new Promise((resolve, reject) => {
        const link = document.createElement('link');
        link.rel = 'stylesheet';
        link.type = 'text/css';
        link.onload = () => resolve();
        link.onerror = () => reject();
        link.href = resourceUrl;
        document.head.appendChild(link);
    });
}
// 加载js文件
function loadScript(resourceUrl, async) {
    const wcScript = document.createElement('script');
    wcScript.type = 'module';
    wcScript.src = resourceUrl;
    wcScript.async = async;
    document.body.appendChild(wcScript);
}

export { loadScript, loadStyle }