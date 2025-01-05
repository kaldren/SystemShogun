using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Movies.Blazor.Client.Pages;
using Movies.Blazor.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpClient("MoviesApi", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["ApiUrl"]!);

    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.UserAgent, "KaloyanDrenskiLaptop");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Movies.Blazor.Client._Imports).Assembly);

app.Run();
