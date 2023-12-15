using System.Net.Mime;

namespace Biwen.Blazor.Components
{
    public partial class MarkdownEditor : ComponentBase, IAsyncDisposable
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; } = null!;

        /// <summary>
        /// 是否支持上传图片,默认不支持
        /// </summary>
        [Parameter]
        public bool UploadImage { get; set; } = false;

        [Parameter]
        public string? UploadImagePath { get; set; } = default!;

        /// <summary>
        /// 默认2048. 单位为kb,2048表示2M
        /// </summary>
        [Parameter]
        public int ImageMaxSize { get; set; } = 2048;

        /// <summary>
        /// 接受的上传文件格式默认 image/png,image/jpeg,使用半角逗号隔开
        /// </summary>
        [Parameter]
        public string ImageAccept { get; set; } = "image/png,image/jpeg";

        [Parameter]
        public string Content { private get; set; } = null!;

        public async ValueTask<string> GetContent()
        {
            if (Module is not null)
            {
                var real = await Module.InvokeAsync<string>("Editor.GetVal");
                Content = real;
            }

            return Content;

        }

        private ValueTask SetContent(string? val)
        {
            if (Module is not null)
            {
                return Module.InvokeVoidAsync("Editor.SetVal", val);
            }
            return ValueTask.CompletedTask;
        }

        private IJSObjectReference? Module;

        /// <summary>
        /// Id
        /// </summary>
        protected readonly string Id = $"editor{Random.Shared.NextInt64()}";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var options = new
                {
                    uploadImage = UploadImage,
                    imageUploadEndpoint = UploadImagePath,
                    imageMaxSize = ImageMaxSize * 1024,
                    imageAccept = ImageAccept,
                    //imageUploadFunction= 
                };

                Module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Biwen.Blazor.Components/modules-easymde.js");
                await Module.InvokeVoidAsync("Editor.Init", Id, options);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (Module is not null)
            {
                try
                {
                    await Module.InvokeVoidAsync("Editor.Dispose");
                    await Module.DisposeAsync();
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}