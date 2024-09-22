// Licensed to the Biwen.Blazor.Components under one or more agreements.
// The Biwen.Blazor.Components licenses this file to you under the MIT license. 
// See the LICENSE file in the project root for more information.
// Biwen.Blazor.Components Author: 万雅虎, Github: https://github.com/vipwan
// Biwen.Blazor.Components
// Modify Date: 2024-09-23 00:07:49 CodeEditor.Interop.cs

namespace Biwen.Blazor.Components;

internal class CodeEditorInterop(IJSRuntime jSRuntime, string domId) : IAsyncDisposable
{
    /// <summary>
    /// JSRuntime
    /// </summary>
    private readonly IJSRuntime _jSRuntime = jSRuntime;
    /// <summary>
    /// Id
    /// </summary>
    private readonly string Id = domId;

    private IJSObjectReference Module = null!;

    public bool IsInitialized { get; private set; }


    public async Task InitAsync(object interop, object options)
    {
        Module ??= await _jSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Biwen.Blazor.Components/modules-monaco-editor.js");

        await Module.InvokeVoidAsync("Monaco.Init", Id, interop, options);
        // 初始化完成
        IsInitialized = true;
    }

    public async Task<string> GetValueAsync()
    {
        if (IsInitialized)
            return await Module.InvokeAsync<string>("Monaco.GetVal");
        return string.Empty;
    }

    public async Task SetValueAsync(string val)
    {
        if (IsInitialized)
            await Module.InvokeVoidAsync("Monaco.SetVal", val);
    }

    public async Task SetOptionsAsync(object options)
    {
        if (IsInitialized)
            await Module.InvokeVoidAsync("Monaco.SetOptions", options);
    }


    public async ValueTask DisposeAsync()
    {
        if (Module is not null)
        {
            try
            {
                await Module.InvokeVoidAsync("Monaco.Dispose");
                await Module.DisposeAsync();
            }
            catch
            {
                // todo:
            }
        }
    }
}