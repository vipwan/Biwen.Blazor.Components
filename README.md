# Biwen.Blazor.Components

## �﷨������� CodeHighlighter
�﷨����ʹ��`Prism.js`��֧��`C#`��`CSS`��`HTML`��`JavaScript`��`JSON`��`Markdown`��`TypeScript`�����ԡ�

- LineNumbersΪtrueʱ��ʾ�к�

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


## MD��ʾ��� MarkdownViewer
�ṩMD�ļ���ʾ����

- FromAsset����ΪMD�ļ�·����Content����ΪMD����
- CodeHighlight=trueʱ����Դ�����и�����ʾ
- CodeHighlightRowNumber=true && CodeHighlight=trueʱ������ʾ�����к�

### 1. ע��HttpClient����
```csharp
//��ע�������Blazor WebAssembly��Ŀ����Ҫ���н����������
builder.Services.AddScoped<HttpClient>();
```
### 2. ʹ��MarkdownViewer���
```razor
<MarkdownViewer FromAsset="/master/README.md" />
<MarkdownViewer Content="* Ҳ������������MD" />
```

## MD�༭�� MarkdownEditor
- Content����ΪMD��ʼ������
- UploadImage=true��ʾ֧���ϴ�ͼƬ,��Ҫͬʱ����`UploadImagePath`(�ϴ���ַ),`ImageMaxSize`(�ļ���С��λkb,Ĭ��2048=2M),`ImageAccept`(֧���ϴ�����,Ĭ��`image/png,image/jpeg`)

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
���`UploadImage`=true����Ҫ�ṩ�ϴ���ַ,������ģ��һ���ϴ��ӿ�:
```csharp

app.MapPost("/upload", ([FromServices] IWebHostEnvironment env, IFormFileCollection files) =>
{
    //��ǰû�з�α���,��Ҫ���д���,��Ҫ���н��Ȩ�޺Ͱ�ȫ����
    //��֧������.ֻ֧��һ���ϴ�һ���ļ�

    //�ϴ��ļ��߼�:
    if (files.Count == 0)
    {
        return Results.Json(new { error = "400" });
    }

    var wwwroot = env.WebRootPath;
    var file = files[0];
    var ext = Path.GetExtension(file.FileName);
    string fileName = $"{Guid.NewGuid()}{ext}";
    //�����Ҫ����Ŀ¼,�����д���
    var filePath = Path.Combine(wwwroot, "uploads", fileName);
    using var stream = new FileStream(filePath, FileMode.CreateNew);
    file.CopyTo(stream);

    return Results.Json(new
    {
        data = new
        {
            //��ע����Ҫʹ�þ��Ե�ַ,Զ��·����ʽ����:http://localhost:5000/uploads/xxx.png
            filePath = $"/uploads/{fileName}"
        }
    });
}).DisableAntiforgery();

```

## Code�༭�� CodeEditor
- Language ����Ϊ�༭�����ԣ�֧��`csharp`��`css`��`html`��`javascript`��`json`��`markdown`��`typescript`������
- Value ����Ϊ�༭����ʼ������
- Style ����Ϊ�༭����ʽ
- ShowLineNumbers�Ƿ���ʾ�к�.Ĭ��true��ʾ
- ValueChanged �༭�����ݸı��¼�
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