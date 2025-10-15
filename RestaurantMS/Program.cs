using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MongoDB.Driver;
using RestaurantMS.Data;
using RestaurantMS.Models;
using RestaurantMS.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDb")
        ?? throw new InvalidOperationException("MongoDB connection string not found.");
    return new MongoClient(connectionString);
});
builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAuthorizationCore();

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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MongoDbContext>();

    var menuCount = await db.MenuItems.CountDocumentsAsync(_ => true);
    if (menuCount == 0)
    {
        var items = new List<MenuItem>
        {
            new MenuItem { Name = "Margherita Pizza", Description = "Classic pizza with cheese & tomato", Price = 7.99M, ImageUrl = "/images/margherita.jpg" },
            new MenuItem { Name = "Pepperoni Pizza", Description = "Pepperoni & cheese", Price = 8.99M, ImageUrl = "/images/pepperoni.jpg" },
            new MenuItem { Name = "Veggie Burger", Description = "Grilled veggie patty with lettuce", Price = 6.49M, ImageUrl = "/images/veggieburger.jpg" },
            new MenuItem { Name = "Cheeseburger", Description = "Beef patty with cheese & toppings", Price = 7.49M, ImageUrl = "/images/cheeseburger.jpg" },
            new MenuItem { Name = "Pasta Alfredo", Description = "Creamy penne pasta with mushrooms", Price = 7.99M, ImageUrl = "/images/pasta.jpg" }
        };
        await db.MenuItems.InsertManyAsync(items);
    }

    var userService = scope.ServiceProvider.GetRequiredService<UserService>();
    var adminEmail = "admin@restaurantms.com";
    var existingAdmin = await userService.GetUserByEmailAsync(adminEmail);
    if (existingAdmin == null)
    {
        var adminUser = new ApplicationUser
        {
            Name = "Admin",
            Email = adminEmail,
            Role = "Manager",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!")
        };
        await userService.CreateUserAsync(adminUser);
    }
}

app.Run();
