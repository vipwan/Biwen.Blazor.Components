# Biwen.Blazor.Components

## �﷨������� PrismHighlighter
�﷨����ʹ��`Prism.js`��֧��`C#`��`CSS`��`HTML`��`JavaScript`��`JSON`��`Markdown`��`SQL`��`TypeScript`��`YAML`�����ԡ�

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