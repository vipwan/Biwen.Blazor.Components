# Biwen.Blazor.Components

## 语法高亮组件 PrismHighlighter
语法高亮使用`Prism.js`，支持`C#`、`CSS`、`HTML`、`JavaScript`、`JSON`、`Markdown`、`SQL`、`TypeScript`、`YAML`等语言。

- LineNumbers为true时显示行号

```razor
<PrismHighlighter LineNumbers="true">
<pre>
	<code class="language-csharp">
	private int currentCount = 0;
	private void IncrementCount()
	{
	currentCount++;
	}
	</code>
</pre>
</PrismHighlighter>
```




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

## MD编辑器 MarkdownEditor
- Content属性为MD初始化内容

```razor
<MarkdownEditor @ref="editor" Content="@md"></MarkdownEditor>
```
```csharp
    private string md = "## Hello World";
    private MarkdownEditor editor = null!;
    private async Task GetContent()
    {
        var content = editor.Content;
        await Task.CompletedTask;
    }
```