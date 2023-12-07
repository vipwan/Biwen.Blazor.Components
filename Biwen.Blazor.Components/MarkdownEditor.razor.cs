namespace Biwen.Blazor.Components
{
    public partial class MarkdownEditor : ComponentBase, IAsyncDisposable
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; } = null!;

        private Timer? _timer;

        protected override Task OnInitializedAsync()
        {
            _timer = new Timer(async (state) =>
            {
                try
                {
                    await GetContent();
                }
                catch
                {
                    // todo:
                }
            }, new AutoResetEvent(false), 3000, 600);

            return Task.CompletedTask;
        }

        protected string? _content;

        /// <summary>
        /// 是否支持上传图片,默认不支持
        /// </summary>
        [Parameter]
        public bool UploadImage { get; set; } = false;

        [Parameter]
        public string? UploadImagePath { get; set; } = default!;






        [Parameter]
        public string? Content
        {
            get
            {
                return _content;
            }
            set
            {
                //_ = SetContent(_content);
                _content = value;
            }
        }

        private async Task GetContent()
        {
            if (Module is not null)
            {
                var real = await Module.InvokeAsync<string>("GetVal", Id);
                if (real != _content)
                {
                    Content = real;
                    // await InvokeAsync(() =>
                    // {
                    //     _content = real;
                    //     OnContentChanged.InvokeAsync(real);
                    // });
                }
            }
        }

        private ValueTask SetContent(string? val)
        {
            if (Module is not null)
            {
                return Module.InvokeVoidAsync("SetVal", Id, val);
            }
            return ValueTask.CompletedTask;
        }

        private IJSObjectReference? Module;

        /// <summary>
        /// Id
        /// </summary>
        private static readonly string Id = $"editor{Random.Shared.NextInt64()}";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var options = new
                {
                    uploadImage = UploadImage,
                    imageUploadEndpoint = UploadImagePath,
                    //imageUploadFunction= 
                };

                Module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Biwen.Blazor.Components/modules-easymde.js");
                await Module.InvokeVoidAsync("Editor.init", Id, options);
            }
        }


        public async ValueTask DisposeAsync()
        {
            if (Module is not null)
            {
                try
                {
                    await Module.DisposeAsync();
                }
                catch
                {
                    // ignored
                }
            }

            if (_timer is not null)
            {
                await _timer.DisposeAsync();
            }
        }
    }
}