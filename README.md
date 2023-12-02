# Biwen.Blazor.Components

## �﷨������� PrismHighlighter
�﷨����ʹ��`Prism.js`��֧��`C#`��`CSS`��`HTML`��`JavaScript`��`JSON`��`Markdown`��`SQL`��`TypeScript`��`YAML`�����ԡ�
```razor
<PrismHighlighter>
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