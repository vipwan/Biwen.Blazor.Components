using Biwen.Blazor.Components;
using Biwen.Blazor.Components.Demo.Shared.Pages;
using Biwen.Blazor.Components.Demo.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddInteractiveWebAssemblyComponents();

builder.Services.AddFluentUIComponents();

// HttpClient
builder.Services.AddScoped(sp => new HttpClient { });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();


// ÉÏ´«
app.MapPost("/upload", (IFormFile file) =>
{
    return Results.Ok("http://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png");
});



app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()

    .AddAdditionalAssemblies(typeof(Doc).Assembly);

app.Run();
