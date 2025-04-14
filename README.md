# Biwen.Blazor.Components

[![NuGet](https://img.shields.io/nuget/v/Biwen.Blazor.Components.svg)](https://www.nuget.org/packages/Biwen.Blazor.Components/)
[![NuGet](https://img.shields.io/nuget/dt/Biwen.Blazor.Components.svg)](https://www.nuget.org/packages/Biwen.Blazor.Components/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## 📝 简介

Biwen.Blazor.Components 是一个功能丰富的 Blazor 组件库，提供多种常用组件，包括代码高亮、Markdown 查看器/编辑器、代码编辑器和二维码生成等功能。项目基于 .NET 9.0 开发，旨在为 Blazor 应用提供高质量、易用的 UI 组件。

## 📦 安装

通过 NuGet 包管理器安装:

```bash
dotnet add package Biwen.Blazor.Components --version 1.0.0-beta4
```

或在包管理器控制台中执行:

```
Install-Package Biwen.Blazor.Components -Version 1.0.0-beta4
```

## 🚀 组件

### 语法高亮组件 CodeHighlighter

语法高亮使用 `Prism.js`，支持 `C#`、`CSS`、`HTML`、`JavaScript`、`JSON`、`Markdown`、`TypeScript` 等多种编程语言。

#### 功能特点
- 支持多种编程语言
- 可配置行号显示

#### 示例代码

```razor
<CodeHighlighter LineNumbers="true">
<pre>
    <code class="language-csharp">
    private int currentCount = 0;
    private void IncrementCount()
    {
        currentCount++;
    }
    </code>
</pre>
</CodeHighlighter>
```


### Markdown显示组件 MarkdownViewer

提供 Markdown 文件显示功能，可以从文件路径或直接内容渲染 Markdown。

#### 功能特点
- 支持从文件路径或直接内容渲染 Markdown
- 代码块语法高亮功能
- 可配置代码块行号显示

#### 使用步骤

##### 1. 注册 HttpClient 服务
```csharp
// 请注意如果是 Blazor WebAssembly 项目，需要自行解决跨域问题
builder.Services.AddScoped<HttpClient>();
```

##### 2. 使用 MarkdownViewer 组件

```razor
<!-- 从文件路径加载 Markdown -->
<MarkdownViewer FromAsset="/master/README.md" CodeHighlight="true" CodeHighlightRowNumber="true" />

<!-- 或者直接传递 Markdown 内容 -->
<MarkdownViewer Content="* 也可以这样传递 Markdown 内容" />
```

### Markdown编辑器 MarkdownEditor

提供功能丰富的 Markdown 编辑体验，支持图片上传等高级功能。

#### 功能特点
- 所见即所得的 Markdown 编辑
- 支持图片上传
- 支持通过引用获取编辑器内容

#### 属性说明
- `Content` - Markdown 初始内容
- `UploadImage` - 是否支持图片上传（默认为 false）
- `UploadImagePath` - 图片上传接口地址
- `ImageMaxSize` - 图片大小限制（单位：KB，默认 2048 = 2MB）
- `ImageAccept` - 支持的图片格式（默认 `image/png,image/jpeg`）

#### 示例代码

```razor
<MarkdownEditor @ref="editor" Content="@md" UploadImage="true" UploadImagePath="/upload"></MarkdownEditor>

@code {
    private string md = "## Hello World";
    private MarkdownEditor editor = null!;
    
    private async Task GetContent()
    {
        var content = editor.Content;
        await Task.CompletedTask;
    }
}
```
#### 图片上传配置

当 `UploadImage=true` 时需要提供上传地址，以下是一个服务端上传接口示例：

```csharp
app.MapPost("/upload", ([FromServices] IWebHostEnvironment env, IFormFileCollection files) =>
{
    // 注意：此示例禁用了防伪标记，生产环境需要处理权限和安全问题
    // 此示例一次只支持上传一个文件
    
    if (files.Count == 0)
    {
        return Results.Json(new { error = "400" });
    }

    var wwwroot = env.WebRootPath;
    var file = files[0];
    var ext = Path.GetExtension(file.FileName);
    string fileName = $"{Guid.NewGuid()}{ext}";
    // 如果需要按日期存储目录，请自行实现
    var filePath = Path.Combine(wwwroot, "uploads", fileName);
    
    // 确保目录存在
    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
    
    using var stream = new FileStream(filePath, FileMode.CreateNew);
    file.CopyTo(stream);

    return Results.Json(new
    {
        data = new
        {
            // 注意：需要使用绝对地址，如 http://localhost:5000/uploads/xxx.png
            filePath = $"/uploads/{fileName}"
        }
    });
}).DisableAntiforgery();

```

### 代码编辑器 CodeEditor (Monaco Editor)

基于 Monaco Editor（VS Code 的编辑器）的代码编辑组件，提供丰富的代码编辑功能。

#### 功能特点
- 支持多种编程语言
- 智能代码高亮
- 行号显示控制
- 内容变更事件
- 多种自定义样式

#### 属性说明
- `Language` - 编辑器语言，支持 `csharp`、`css`、`html`、`javascript`、`json`、`markdown`、`typescript` 等
- `Value` - 编辑器初始内容
- `Style` - 编辑器 CSS 样式
- `ShowLineNumbers` - 是否显示行号（默认 true）
- `ValueChanged` - 编辑器内容改变事件

#### 示例代码
```razor
<CodeEditor 
    @ref="codeEditor" 
    Language="csharp" 
    Value="@initialCode"
    Style="height: 400px; width: 100%;"
    ShowLineNumbers="true"
    @onchange="HandleCodeChange">
</CodeEditor>

@code {
    private string initialCode = "using System;\n\nnamespace Demo\n{\n    public class Program\n    {\n        public static void Main()\n        {\n            Console.WriteLine(\"Hello World!\");\n        }\n    }\n}";
    private CodeEditor codeEditor = null!;

    private void HandleCodeChange(string newValue)
    {
        // 处理代码变更
        Console.WriteLine($"Code changed: {newValue}");
    }
}
```

### 二维码组件 QRCode

用于生成可自定义的二维码。

#### 功能特点
- 简单易用的二维码生成
- 可自定义二维码内容、大小和颜色
- 支持不同错误纠正级别

#### 属性说明
- `Text` - 要编码的文本内容
- `Width` - 二维码宽度（像素）
- `Height` - 二维码高度（像素）
- `ColorDark` - 二维码深色部分的颜色
- `ColorLight` - 二维码浅色部分的颜色
- `ErrorCorrectionLevel` - 错误纠正级别 (L, M, Q, H)

#### 示例代码
```razor
<QRCode Text="https://github.com/vipwan/Biwen.Blazor.Components" 
        Width="200" 
        Height="200"
        ColorDark="#000000"
        ColorLight="#ffffff" />
```

## 🔧 兼容性

- 支持 .NET 9.0+
- 支持 Blazor Server 和 Blazor WebAssembly
- 支持主流现代浏览器

## 🤝 贡献

欢迎提交问题和贡献代码！

## 📄 许可证

本项目基于 MIT 许可证开源 - 详情请参阅 [LICENSE.txt](LICENSE.txt) 文件
