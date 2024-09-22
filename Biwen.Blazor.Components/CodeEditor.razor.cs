// Licensed to the Biwen.Blazor.Components under one or more agreements.
// The Biwen.Blazor.Components licenses this file to you under the MIT license. 
// See the LICENSE file in the project root for more information.
// Biwen.Blazor.Components Author: 万雅虎, Github: https://github.com/vipwan
// Biwen.Blazor.Components
// Modify Date: 2024-09-23 00:08:06 CodeEditor.razor.cs

namespace Biwen.Blazor.Components;

public partial class CodeEditor : ComponentBase, IAsyncDisposable
{

    [Parameter]
    public string? Language { get; set; } = "csharp";

    /// <summary>
    /// Style of the editor
    /// </summary>
    [Parameter]
    public string? Style { get; set; } = "width: 100%; height: 350px; border: 1px solid grey";

    [Parameter]
    public string? Value { get; set; } = "using System;\r\nConsole.WriteLine('hello CodeEditor');";

    [Parameter]
    public bool ShowLineNumbers { get; set; } = true;

    [Parameter]
    public bool ReadOnly { get; set; } = false;


    /// <summary>
    /// 值更新回调
    /// </summary>
    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    CodeEditorInterop Interop = null!;


    protected override async Task OnInitializedAsync()
    {
        Interop = new CodeEditorInterop(JSRuntime, Id);
        await Task.CompletedTask;
    }

    protected readonly string Id = $"codeEditor{Random.Shared.NextInt64()}";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var options = new
        {
            value = Value,
            language = Language,
            theme = "vs",
            lineNumbers = ShowLineNumbers,
            readOnly = ReadOnly,
        };

        if (firstRender)
        {
            await Interop.InitAsync(DotNetObjectReference.Create(this), options);
        }
        else
        {
            await Interop.SetOptionsAsync(options);
        }
    }

    public async ValueTask<string> GetValueAsync()
    {
        return await Interop.GetValueAsync();
    }

    public async Task SetValueAsync(string value)
    {
        await Interop.SetValueAsync(value);
    }

    [JSInvokable]
#pragma warning disable
    public async Task OnEditorLoad()
#pragma warning restore
    {
        await Task.CompletedTask;

        //var options = new
        //{
        //    value = Value,
        //    language = Language,
        //    theme = "vs",
        //    lineNumbers = ShowLineNumbers,
        //    readOnly = ReadOnly,
        //};

        //await Interop.SetOptions(options);
    }

    [JSInvokable]
    public async Task UpdateValueAsync(string value)
    {
        Value = value;
        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(Value);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await Interop.DisposeAsync();
    }
}