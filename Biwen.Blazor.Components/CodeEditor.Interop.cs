namespace Biwen.Blazor.Components
{
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


        public async Task Init(object interop, object options)
        {
            Module ??= await _jSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Biwen.Blazor.Components/modules-monaco-editor.js");

            await Module.InvokeVoidAsync("Monaco.Init", Id, interop, options);
            // 初始化完成
            IsInitialized = true;
        }

        public async Task<string> GetValue()
        {
            if (IsInitialized)
                return await Module.InvokeAsync<string>("Monaco.GetVal");
            return string.Empty;
        }

        public async Task SetValue(string val)
        {
            if (IsInitialized)
                await Module.InvokeVoidAsync("Monaco.SetVal", val);
        }

        public async Task SetOptions(object options)
        {
            if (IsInitialized)
                await Module.InvokeVoidAsync("Monaco.SetOptions", options);
        }


        public async ValueTask DisposeAsync()
        {
            if (Module is not null)
            {
                await Module.InvokeVoidAsync("Monaco.Dispose");
                await Module.DisposeAsync();
            }
        }
    }


}