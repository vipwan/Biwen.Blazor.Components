using Biwen.Blazor.Components.Demo.Components;
using Biwen.Blazor.Components.Demo.Shared.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddInteractiveWebAssemblyComponents();

builder.Services.AddFluentUIComponents();

// SignalR
builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024;
});


// HttpClient
builder.Services.AddHttpClient();

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();


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

//net 9.0 +
app.MapStaticAssets();

app.UseAntiforgery();

// 上传
app.MapPost("/upload", ([FromServices] IWebHostEnvironment env, IFormFileCollection files) =>
{
    //当前没有防伪标记,需要自行处理
    //不支持批量.只支持一次上传一个文件
    //需要自行解决权限和安全问题
    //上传文件逻辑:

    //imageUploadEndpoint: The endpoint where the images data will be sent,
    //via an asynchronous POST request.The server is supposed to save this image, and return a JSON response.
    //if the request was successfully processed(HTTP 200 OK): { "data": { "filePath": "<filePath>"} }
    //where filePath is the path of the image (absolute if imagePathAbsolute is set to true, relative if otherwise);
    //otherwise: { "error": "<errorCode>"}, where errorCode can be noFileGiven (HTTP 400 Bad Request),
    //typeNotAllowed (HTTP 415 Unsupported Media Type), fileTooLarge (HTTP 413 Payload Too Large) or importError (see errorMessages below).
    //If errorCode is not one of the errorMessages, it is alerted unchanged to the user.
    //This allows for server-side error messages. No default value.

    if (files.Count == 0)
    {
        return Results.Json(new { error = "400" });
    }

    var wwwroot = env.WebRootPath;
    var file = files[0];
    var ext = Path.GetExtension(file.FileName);
    string fileName = $"{Guid.NewGuid()}{ext}";

    #region 如果文件夹不存在.创建文件夹
    var dir = Path.Combine(wwwroot, "uploads");
    if (!Directory.Exists(dir))
    {
        Directory.CreateDirectory(dir);
    }
    #endregion

    //如果需要日期目录,请自行处理
    var filePath = Path.Combine(wwwroot, "uploads", fileName);
    using var stream = new FileStream(filePath, FileMode.CreateNew);
    file.CopyTo(stream);

    return Results.Json(new
    {
        data = new
        {
            //请注意需要使用绝对地址,远程路径格式比如:http://localhost:5000/uploads/xxx.png
            filePath = $"/uploads/{fileName}"
        }
    });
}).DisableAntiforgery();

app.MapRazorComponents<App>()
.AddInteractiveServerRenderMode()
.AddInteractiveWebAssemblyRenderMode()
.AddAdditionalAssemblies(typeof(Doc).Assembly);

app.Run();
