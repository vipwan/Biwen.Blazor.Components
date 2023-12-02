## MD显示组件 MarkdownViewer
提供MD文件显示功能

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