namespace Biwen.Blazor.Components
{
    public partial class PrismHighlighter : ComponentBase ,IAsyncDisposable
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = null!;

        private ElementReference _elementRef;

        private IJSObjectReference? _module;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _module = await JSRuntime.InvokeAsync<IJSObjectReference>(
                    "import",
                    "./_content/Biwen.Blazor.Components/modules-PrismHighlighter.js");
            }

            if (_module is not null)
                await _module.InvokeVoidAsync("highlight", _elementRef, true);
        }

        public async ValueTask DisposeAsync()
        {
            if (_module is not null)
            {
                try
                {
                    await _module.DisposeAsync();
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}