using Markdig;
using System.Text;

namespace Biwen.Blazor.Components
{
    public partial class MarkdownViewer : ComponentBase, IAsyncDisposable
    {

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        private string? _content;
        private bool _raiseContentConverted;

        [Inject]
        private HttpClient HttpClient { get; set; } = default!;

        /// <summary>
        /// 默认: Encoding.UTF8
        /// </summary>
        [Parameter]
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 是否代码高亮,默认高亮
        /// </summary>
        [Parameter]
        public bool CodeHighlight { get; set; } = true;

        /// <summary>
        /// 代码高亮行号,默认显示
        /// </summary>
        [Parameter]
        public bool CodeHighlightRowNumber { get; set; } = true;


        /// <summary>
        /// Gets or sets the Markdown content
        /// </summary>
        [Parameter]
        public string? Content { get; set; }

        /// <summary>
        /// 资源文件路径格式: /assets/xxx.md,
        /// 设计到跨域问题,暂不支持远程资源
        /// </summary>
        [Parameter]
        public string? FromAsset { get; set; }

        [Parameter]
        public EventCallback OnContentConverted { get; set; }

        public string? InternalContent
        {
            get => _content;
            set
            {
                _content = value;
                HtmlContent = ConvertToMarkupString(_content);

                if (OnContentConverted.HasDelegate)
                {
                    OnContentConverted.InvokeAsync();
                }
                _raiseContentConverted = true;
                StateHasChanged();
            }
        }

        public MarkupString HtmlContent { get; private set; }


        protected override async Task OnInitializedAsync()
        {
            if (Content is null && string.IsNullOrEmpty(FromAsset))
                throw new ArgumentException("You need to provide either Content or FromAsset parameter");
            InternalContent = Content;
            await Task.CompletedTask;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (string.IsNullOrEmpty(InternalContent) && !string.IsNullOrEmpty(FromAsset))
            {
                var url = $"{NavigationManager.BaseUri}{FromAsset}";
                var bytes = await HttpClient.GetByteArrayAsync(url);
                InternalContent = Encoding.GetString(bytes);
            }

            if (_raiseContentConverted)
            {
                _raiseContentConverted = false;
                if (OnContentConverted.HasDelegate)
                {
                    await OnContentConverted.InvokeAsync();
                }
            }
        }

        private static MarkupString ConvertToMarkupString(string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                // Convert markdown string to HTML
                string? html = Markdown.ToHtml(value, new MarkdownPipelineBuilder().UseAdvancedExtensions().Build());
                return new MarkupString(html);
            }
            return new MarkupString();
        }

        public async ValueTask DisposeAsync()
        {
            await Task.CompletedTask;
        }
    }
}