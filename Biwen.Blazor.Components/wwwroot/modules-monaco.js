/*
 * @Author: 万雅虎
 * @Date: 2025-04-11 22:14:04
 * @LastEditTime: 2025-04-14 02:59:52
 * @LastEditors: 万雅虎
 * @Description: Monaco编辑器JavaScript模块
 * @FilePath: \Biwen.Blazor.Components\Biwen.Blazor.Components\wwwroot\modules-monaco.js
 * vipwan@sina.com © 万雅虎
 */

// Monaco编辑器模块
const MonacoEditor = {
    // 已初始化的编辑器实例
    _instances: {},

    // 加载Monaco编辑器脚本
    _loadMonacoScript: async function () {
        return new Promise((resolve, reject) => {
            const script = document.createElement('script');
            script.src = '_content/Biwen.Blazor.Components/monaco-editor/min/vs/loader.js';
            script.onload = resolve;
            script.onerror = reject;
            document.head.appendChild(script);
        });
    },

    // 初始化编辑器
    init: async function (id, options, dotNetRef) {
        try {
            // 如果require未定义，加载Monaco加载器脚本
            if (typeof require === 'undefined') {
                await this._loadMonacoScript();
                await new Promise(resolve => setTimeout(resolve, 500)); // 增加等待时间，确保加载器完全初始化
            }

            // 确保require已定义
            if (typeof require === 'undefined') {
                throw new Error('Monaco加载器初始化失败');
            }

            // 配置Monaco加载器
            require.config({
                paths: {
                    'vs': '_content/Biwen.Blazor.Components/monaco-editor/min/vs'
                },
                // 配置Web Worker
                'vs/nls': {
                    availableLanguages: {
                        '*': 'zh-cn'
                    }
                }
            });

            // 配置Monaco环境
            window.MonacoEnvironment = {
                getWorkerUrl: function (_workerId, _label) {
                    // 使用data URL加载Web Worker
                    return `data:text/javascript;charset=utf-8,${encodeURIComponent(`
                        self.MonacoEnvironment = {
                            baseUrl: '_content/Biwen.Blazor.Components/monaco-editor/min/'
                        };
                    `)}`;
                }
            };

            // 加载Monaco编辑器
            require(['vs/editor/editor.main'], () => {
                try {
                    // 获取容器元素
                    const container = document.getElementById(id);
                    if (!container) {
                        throw new Error(`找不到编辑器容器: ${id}`);
                    }

                    // 检查是否已经存在编辑器实例
                    if (this._instances[id]) {
                        console.log(`编辑器实例已存在，正在销毁旧实例: ${id}`);
                        try {
                            this._instances[id].dispose();
                        } catch (disposeError) {
                            console.warn(`销毁旧编辑器实例失败: ${disposeError}`);
                        }
                        delete this._instances[id];
                    }

                    // 创建编辑器实例
                    const editorOptions = {
                        value: options.value || '',
                        language: options.language || 'csharp',
                        theme: options.theme || 'vs',
                        automaticLayout: true,
                        roundedSelection: true,
                        scrollBeyondLastLine: false,
                        readOnly: options.readOnly || false,
                        fontSize: options.fontSize || 14,
                    };

                    // 处理lineNumbers选项
                    if (options.lineNumbers !== undefined) {
                        editorOptions.lineNumbers = options.lineNumbers;
                    }

                    // 处理minimap选项
                    if (options.minimap !== undefined) {
                        editorOptions.minimap = options.minimap;
                    }

                    // 确保语言服务已加载
                    if (options.language && monaco.languages) {
                        // 注册语言服务（如果尚未注册）
                        if (!monaco.languages.getLanguages().some(lang => lang.id === options.language)) {
                            console.log(`注册语言: ${options.language}`);
                            // 这里可以添加特定语言的注册逻辑
                        }
                    }

                    // 创建编辑器
                    const editor = monaco.editor.create(container, editorOptions);

                    // 调试信息
                    console.log('Monaco Editor Options:', editorOptions);

                    // 监听内容变化事件
                    editor.onDidChangeModelContent(() => {
                        if (dotNetRef) {
                            dotNetRef.invokeMethodAsync('OnValueChanged', editor.getValue());
                        }
                    });

                    // 存储编辑器实例
                    this._instances[id] = editor;

                    // 通知Blazor编辑器已加载
                    if (dotNetRef) {
                        dotNetRef.invokeMethodAsync('OnEditorLoaded');
                    }
                } catch (innerError) {
                    console.error('创建Monaco编辑器实例失败:', innerError);
                    if (dotNetRef) {
                        dotNetRef.invokeMethodAsync('OnEditorError', innerError.toString());
                    }
                }
            });
        } catch (error) {
            console.error('Monaco编辑器初始化失败:', error);
            if (dotNetRef) {
                dotNetRef.invokeMethodAsync('OnEditorError', error.toString());
            }
            throw error;
        }
    },

    // 获取编辑器值
    getValue: function (id) {
        const editor = this._instances[id];
        return editor ? editor.getValue() : '';
    },

    // 设置编辑器值
    setValue: function (id, value) {
        const editor = this._instances[id];
        if (editor) {
            try {
                // 获取当前语言
                const model = editor.getModel();
                const language = model ? model.getLanguageId() : null;

                // 设置值
                editor.setValue(value || '');

                // 确保语言设置正确
                if (language) {
                    // 重新应用语言，强制刷新语法高亮
                    monaco.editor.setModelLanguage(model, language);
                }
            } catch (error) {
                console.error('设置编辑器值失败:', error);
            }
        }
    },

    // 更新编辑器选项
    updateOptions: function (id, options) {
        const editor = this._instances[id];
        if (editor) {
            try {
                // 创建一个新的选项对象
                const editorOptions = {};

                // 复制原始选项
                Object.assign(editorOptions, options);

                // 处理minimap选项
                if (options.minimap !== undefined) {
                    if (typeof options.minimap === 'boolean') {
                        editorOptions.minimap = { enabled: options.minimap };
                    } else if (typeof options.minimap === 'object' && options.minimap !== null) {
                        // 已经是对象形式，保持不变
                        editorOptions.minimap = options.minimap;
                    }
                }

                // 处理lineNumbers选项
                if (options.lineNumbers !== undefined) {
                    if (typeof options.lineNumbers === 'boolean') {
                        editorOptions.lineNumbers = options.lineNumbers ? 'on' : 'off';
                    } else if (typeof options.lineNumbers === 'string') {
                        // 已经是字符串形式，保持不变
                        editorOptions.lineNumbers = options.lineNumbers;
                    }
                }

                // 调试信息
                console.log('Updating editor options:', editorOptions);

                // 更新编辑器选项
                editor.updateOptions(editorOptions);

                // 如果有语言选项，确保语言正确应用
                if (options.language) {
                    const model = editor.getModel();
                    if (model) {
                        // 重新应用语言，强制刷新语法高亮
                        monaco.editor.setModelLanguage(model, options.language);
                        console.log(`已重新应用语言: ${options.language}`);
                    }
                }
            } catch (error) {
                console.error('Error updating editor options:', error);
            }
        }
    },

    // 格式化代码
    formatDocument: function (id) {
        const editor = this._instances[id];
        if (editor) {
            editor.getAction('editor.action.formatDocument')?.run();
        }
    },

    // 添加自定义操作
    addAction: function (id, actionId, label, keybinding, dotNetRef) {
        const editor = this._instances[id];
        if (editor) {
            editor.addAction({
                id: actionId,
                label: label,
                keybindings: [this._parseKeybinding(keybinding)],
                run: function () {
                    if (dotNetRef) {
                        dotNetRef.invokeMethodAsync('OnActionExecuted', actionId);
                    }
                    return null;
                }
            });
        }
    },

    // 解析键绑定字符串为Monaco键绑定代码
    _parseKeybinding: function (keybinding) {
        if (!keybinding) return null;

        const parts = keybinding.toLowerCase().split('+');
        let result = 0;

        for (const part of parts) {
            switch (part.trim()) {
                case 'ctrl': result |= monaco.KeyMod.CtrlCmd; break;
                case 'shift': result |= monaco.KeyMod.Shift; break;
                case 'alt': result |= monaco.KeyMod.Alt; break;
                case 'cmd': result |= monaco.KeyMod.WinCtrl; break;
                default:
                    // 处理单个键
                    if (part.length === 1) {
                        // 字母和数字
                        const code = part.toUpperCase().charCodeAt(0);
                        result |= code;
                    } else {
                        // 功能键等
                        const keyCode = monaco.KeyCode[part.toUpperCase()];
                        if (keyCode) {
                            result |= keyCode;
                        }
                    }
            }
        }

        return result;
    },

    // 销毁编辑器实例
    dispose: function (id) {
        try {
            const editor = this._instances[id];
            if (editor) {
                console.log(`正在销毁编辑器实例: ${id}`);
                try {
                    // 销毁编辑器模型
                    const model = editor.getModel();
                    if (model) {
                        model.dispose();
                    }
                } catch (modelError) {
                    console.warn(`销毁编辑器模型失败: ${modelError}`);
                }

                try {
                    // 销毁编辑器实例
                    editor.dispose();
                } catch (editorError) {
                    console.warn(`销毁编辑器实例失败: ${editorError}`);
                }

                // 从实例字典中移除
                delete this._instances[id];
                console.log(`编辑器实例已销毁: ${id}`);
            }
        } catch (error) {
            console.error(`销毁编辑器时发生错误: ${error}`);
        }
    }
};

// 导出模块
export { MonacoEditor };
