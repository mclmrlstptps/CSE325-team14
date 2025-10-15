using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using MongoDB.Driver;
using RestaurantMS.Data;
using RestaurantMS.Services;

var builder = WebApplication.CreateBuilder(args);

// MongoDB Configuration
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDbConnection")!;
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("MongoDb connection string is missing in appsettings.json!");
    }
    return new MongoClient(connectionString);
});

builder.Services.AddScoped<MongoDbContext>();
builder.Services.AddScoped<MongoDBService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<MenuItemService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Enable authentication & authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
