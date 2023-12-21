using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biwen.Blazor.Components
{
    /// <summary>
    /// QRCodeLevel
    /// </summary>
    public enum QRCodeLevel
    {
        L,
        /// <summary>
        /// Default
        /// </summary>
        M,
        Q,
        H
    }


    public partial class QRCode : ComponentBase, IAsyncDisposable
    {

        protected readonly string Id = $"qrcode{Random.Shared.NextInt64()}";

        /// <summary>
        /// Content
        /// </summary>
        [Parameter, EditorRequired]
        public string Content { get; set; } = null!;

        /// <summary>
        /// 二维码基准点默认4-10
        /// </summary>
        [Parameter]
        public int TypeNumber { get; set; } = 4;

        /// <summary>
        /// 二维码尺寸默认4=164px
        /// </summary>
        [Parameter]
        public int CellSize { get; set; } = 4;

        /// <summary>
        /// Level ('L', 'M', 'Q', 'H')
        /// </summary>
        [Parameter]
        public QRCodeLevel Level { get; set; } = QRCodeLevel.M;


        [Parameter]
        public RenderFragment? ChildContent { get; set; }


        [Inject]
        private IJSRuntime JSRuntime { get; set; } = null!;

        private IJSObjectReference? _module;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _module ??= await JSRuntime.InvokeAsync<IJSObjectReference>(
               "import",
               "./_content/Biwen.Blazor.Components/modules-qrcode.js");
            }

            // Init(element, typeNumber, level, content,cellSize) 
            if (_module is not null)
                await _module.InvokeVoidAsync("InitQRcode", Id, TypeNumber.ToString(), Level.ToString(), Content, CellSize);
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