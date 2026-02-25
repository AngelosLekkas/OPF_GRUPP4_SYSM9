using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// 1. Add DbContext (SQL Server)
// -----------------------------
builder.Services.AddDbContext<CyberQuizDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------------------------------
// 2. Add Identity (Users + Roles + EF Storage)
// ---------------------------------------------
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<CyberQuizDbContext>()
    .AddDefaultTokenProviders();

// -----------------------------
// 3. Add Authentication system
// (JWT kommer soooooon)
// -----------------------------
builder.Services.AddAuthentication();

// -----------------------------
// 4. Add Controllers
// -----------------------------
builder.Services.AddControllers();

// -----------------------------
// 5. Add OpenAPI / Swagger
// -----------------------------
builder.Services.AddOpenApi();

// -----------------------------
// 6. Add CORS (för Blazor UI)
// -----------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .WithOrigins("https://localhost:7200", "http://localhost:7200");  // Check carefully the ports your Blazor UI is running on :D
    });
});

var app = builder.Build();


// -----------------------------
// 7. Run Seeder (Roles + User + Quiz Data)
// -----------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CyberQuizDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await DbSeeder.SeedAsync(db, userManager, roleManager);
}




// -----------------------------
// 8. Middleware pipeline
// -----------------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowUI");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();