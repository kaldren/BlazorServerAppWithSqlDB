using DemoBlazorServerApp;
using DemoBlazorServerApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add the UserService into the DI container
builder.Services.AddScoped<UserService>();

// Here we use the environment variables we've set up in the docker-compose.yml file
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer($"Server={builder.Configuration["DB_HOST"]};Database={builder.Configuration["DB_NAME"]};User Id={builder.Configuration["DB_USER"]};Password={builder.Configuration["DB_SA_PASSWORD"]};"));

var app = builder.Build();

// Ensure the database is created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated(); // This creates the database if it doesn't exist

        // Add demo users
        var usersToAdd = new List<User>
        {
            new User { Name = "John Doe", Email = "johndoe@example.com" },
            new User { Name = "Bob Doe", Email = "bobdoe@example.com" }
        };

        context.Users.AddRange(usersToAdd); // Add new users
        context.SaveChanges(); // Save changes to the database
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
