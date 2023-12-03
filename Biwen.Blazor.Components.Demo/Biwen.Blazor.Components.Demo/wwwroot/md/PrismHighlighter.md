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