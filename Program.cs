using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RestaurantMS.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// import MongoDB service
builder.Services.AddSingleton<MongoDBService>();

// import Auth service
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<MenuService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
