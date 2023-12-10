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
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddInteractiveWebAssemblyComponents();

builder.Services.AddFluentUIComponents();

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
app.UseStaticFiles();
app.UseAntiforgery();


// �ϴ�
app.MapPost("/upload", ([FromServices] IWebHostEnvironment env, IFormFileCollection files) =>
{
    //��ǰû�з�α���,��Ҫ���д���
    //��֧������.ֻ֧��һ���ϴ�һ���ļ�
    //��Ҫ���н��Ȩ�޺Ͱ�ȫ����
    //�ϴ��ļ��߼�:

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

    #region ����ļ��в�����.�����ļ���
    var dir = Path.Combine(wwwroot, "uploads");
    if (!Directory.Exists(dir))
    {
        Directory.CreateDirectory(dir);
    }
    #endregion

    //�����Ҫ����Ŀ¼,�����д���
    var filePath = Path.Combine(wwwroot, "uploads", fileName);
    using var stream = new FileStream(filePath, FileMode.CreateNew);
    file.CopyTo(stream);

    return Results.Json(new
    {
        data = new
        {
            //��ע����Ҫʹ�þ��Ե�ַ,Զ��·����ʽ����:http://localhost:5000/uploads/xxx.png
            filePath = $"/uploads/{fileName}"
        }
    });
}).DisableAntiforgery();

app.MapRazorComponents<App>()
.AddInteractiveServerRenderMode()
.AddInteractiveWebAssemblyRenderMode()
.AddAdditionalAssemblies(typeof(Doc).Assembly);

app.Run();
