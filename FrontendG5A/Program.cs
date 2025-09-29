using FrontendG5A.Components;
using FrontendG5A.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Scoped
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api-gateway-8wvg.onrender.com/") });

builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<BlogService>();

builder.Services.AddScoped<UsuarioService>();

builder.Services.AddScoped<ProductoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
//extra inicio
app.MapFallbackToFile("index.html");
//extra fin
app.Run();
