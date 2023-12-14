# Biwen.Blazor.Components

## 语法高亮组件 CodeHighlighter
语法高亮使用`Prism.js`，支持`C#`、`CSS`、`HTML`、`JavaScript`、`JSON`、`Markdown`、`TypeScript`等语言。

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
- UploadImage=true表示支持上传图片,需要同时设置`UploadImagePath`(上传地址),`ImageMaxSize`(文件大小单位kb,默认2048=2M),`ImageAccept`(支持上传类型,默认`image/png,image/jpeg`)

```razor
<MarkdownEditor @ref="editor" Content="@md" UploadImage="true" UploadImagePath="/upload"></MarkdownEditor>
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
如果`UploadImage`=true还需要提供上传地址,以下是模拟一个上传接口:
```csharp

app.MapPost("/upload", ([FromServices] IWebHostEnvironment env, IFormFileCollection files) =>
{
    //当前没有防伪标记,需要自行处理,需要自行解决权限和安全问题
    //不支持批量.只支持一次上传一个文件

    //上传文件逻辑:
    if (files.Count == 0)
    {
        return Results.Json(new { error = "400" });
    }

    var wwwroot = env.WebRootPath;
    var file = files[0];
    var ext = Path.GetExtension(file.FileName);
    string fileName = $"{Guid.NewGuid()}{ext}";
    //如果需要日期目录,请自行处理
    var filePath = Path.Combine(wwwroot, "uploads", fileName);
    using var stream = new FileStream(filePath, FileMode.CreateNew);
    file.CopyTo(stream);

    return Results.Json(new
    {
        data = new
        {
            //请注意需要使用绝对地址,远程路径格式比如:http://localhost:5000/uploads/xxx.png
            filePath = $"/uploads/{fileName}"
        }
    });
}).DisableAntiforgery();

```

## Code编辑器 CodeEditor
- Language 属性为编辑器语言，支持`csharp`、`css`、`html`、`javascript`、`json`、`markdown`、`typescript`等语言
- Value 属性为编辑器初始化内容
- Style 属性为编辑器样式
- ShowLineNumbers是否显示行号.默认true显示
- ValueChanged 编辑器内容改变事件
```razor
 <CodeEditor @ref="codeEditor" Language="csharp"></CodeEditor>
```
```csharp
    private CodeEditor codeEditor = null!;
    private async Task GetContent()
    {
        var content = codeEditor.Value;
        await Task.CompletedTask;
    }
```

## 二维码生成器 QRCode
- Content 属性为二维码内容
- Level 属性为二维码清晰度，支持`L`、`M`、`Q`、`H`四个级别 默认M
- TypeNumber 属性为二维码容错级别，支持1-40，数字越大，二维码容错率越高 默认4
- CellSize 属性为二维码单元格大小，数字越大，二维码越大 默认4 ≈ 164px, 5 ≈ 205px

```razor
<QRCode Content="hello biwen" Level="@QRCodeLevel.H" TypeNumber="4" CellSize="5"></QRCode>
```
