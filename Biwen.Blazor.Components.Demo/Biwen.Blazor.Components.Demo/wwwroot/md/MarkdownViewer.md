## MD显示组件 MarkdownViewer
提供MD文件显示功能

- FromAsset属性为MD文件路径，Content属性为MD内容
- CodeHighlight=true时，会对代码进行高亮显示
- CodeHighlightRowNumber=true && CodeHighlight=true时，会显示代码行号

### 1. 注册HttpClient服务
```csharp
//请注意如果是Blazor WebAssembly项目，需要自行解决跨域问题
builder.Services.AddScoped<HttpClient>();
```
### 2. 使用MarkdownViewer组件
```razor
<MarkdownViewer FromAsset="/master/README.md" />
<MarkdownViewer Content="* 也可以这样传递MD" />
```