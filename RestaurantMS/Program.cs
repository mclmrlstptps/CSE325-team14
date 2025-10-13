using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MongoDB.Driver;
using RestaurantMS.Data;
using RestaurantMS.Services;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

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
