using Biwen.Blazor.Components;
using Biwen.Blazor.Components.Demo.Shared.Pages;
using Biwen.Blazor.Components.Demo.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


// 上传
app.MapPost("/upload", () =>
{
    //上传文件逻辑:


    //imageUploadEndpoint: The endpoint where the images data will be sent,
    //via an asynchronous POST request.The server is supposed to save this image, and return a JSON response.
    //if the request was successfully processed(HTTP 200 OK): { "data": { "filePath": "<filePath>"} }
    //where filePath is the path of the image (absolute if imagePathAbsolute is set to true, relative if otherwise);
    //otherwise: { "error": "<errorCode>"}, where errorCode can be noFileGiven (HTTP 400 Bad Request),
    //typeNotAllowed (HTTP 415 Unsupported Media Type), fileTooLarge (HTTP 413 Payload Too Large) or importError (see errorMessages below).
    //If errorCode is not one of the errorMessages, it is alerted unchanged to the user.
    //This allows for server-side error messages. No default value.

    return Results.Json(new
    {
        data = new
        {
            filePath = "http://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png"
        }
    });
});

app.MapRazorComponents<App>()
.AddInteractiveServerRenderMode()
.AddInteractiveWebAssemblyRenderMode()

    .AddAdditionalAssemblies(typeof(Doc).Assembly);

app.Run();
