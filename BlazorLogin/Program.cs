using BlazorLogin.Data;
using BlazorLogin.Data.Context;
using BlazorLogin.Data.Context.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();


builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, StateProvider>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<StateProvider>();

builder.Services.AddDbContext<BlazorLogin.Data.Context.DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<BlazorLogin.Data.Context.DbContext>()
    .AddDefaultTokenProviders();

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.Cookie.HttpOnly = true;
//        // ... other cookie options
//    });

builder.Services.AddTransient<Service>();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options =>
    {
        options.DisconnectedCircuitMaxRetained = 100;
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
        options.JSInteropDefaultCallTimeout = TimeSpan.FromSeconds(60);
    });

//builder.Services.AddAuthorization();

//builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication(); 
app.UseAuthorization(); 
app.UseRouting();

// Add the custom middleware to set the cookie
//app.Use(async (context, next) =>
//{
//    context.Response.Cookies.Append("MyCookieName", "MyCookieValue", new CookieOptions
//    {
//        HttpOnly = true,
//        Secure = true, // Set to true in production
//        SameSite = SameSiteMode.Strict,
//        Expires = DateTime.UtcNow.AddDays(7)
//    });

//    await next.Invoke();
//});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();