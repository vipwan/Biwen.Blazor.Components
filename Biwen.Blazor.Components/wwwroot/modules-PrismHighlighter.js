import './prism.js';

//import Loader from './modules-Loader.js';

function highlight(element, flag) {
    //Loader.loadStyle('./_content/Biwen.Blazor.Components/prism.css');
    //loadScript('./_content/Biwen.Blazor.Components/prism.js', true);
    //Prism.highlightAllUnder(element, flag);
    Prism.highlightAll();
}


export { highlight };