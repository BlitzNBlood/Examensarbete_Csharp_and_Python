using System.IO;
using Microsoft.AspNetCore.DataProtection;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ApiService>();
builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, "keys")));
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();
app.UseRouting();
app.UseStaticFiles();
app.MapRazorPages();
app.UseAntiforgery();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.MapGet("/", (HttpContext context) =>
{
    context.Response.Redirect("/taxi");
    return Task.CompletedTask;
});

app.Run();