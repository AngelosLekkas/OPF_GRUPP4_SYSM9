using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Entities;
using CyberQuiz.DAL.Repositories;
using CyberQuiz.DAL.Repositories.Interfaces;
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
// Also enable Swagger generator so Swagger UI is available for manual testing
builder.Services.AddSwaggerGen();

// -----------------------------
// 6. Add CORS (för Blazor UI)
// -----------------------------
builder.Services.AddCors(options =>
{   // Client Policy - För Blazor UI
    options.AddPolicy("AllowUI", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()

              // Blazor URL, Check carefully the ports your Blazor UI is running on :D
              /*.WithOrigins("https://localhost:7255"); */

              .WithOrigins("https://localhost:7250"); 
    });
});



// -----------------------------
// 7. Register Repositories + UnitOfWork
// -----------------------------
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerOptionRepository, AnswerOptionRepository>();
builder.Services.AddScoped<IUserResultRepository, UserResultRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();


// -----------------------------
// 8. Run Seeder (Roles + User + Quiz Data)
// -----------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CyberQuizDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await DbSeeder.SeedAsync(db, userManager, roleManager);
}




// -----------------------------
// 9. Middleware pipeline
// -----------------------------
if (app.Environment.IsDevelopment())
{
    // Map minimal OpenAPI endpoints (project helper) and enable the Swagger UI
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CyberQuiz API V1");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowUI");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


// UI <--> API (Refererar inte till varandra, peka på varandra via CORS och baseAdress)
// API --> BLL & Shared (Class Library)
// BLL -> DAL & Shared
// DAL -> Shared

//SCENARIO
// Användare som svara på en fråga
// 1. UI > skaickar svaret till API
// 2. API > skickar till BLL 
// 3. BLL > Räkna rätt / fel
// 4. BLL -> Säg till DAL att spara
// 5. DAL -> Sparar till databasen
// 6. BLL > Räkna progression
// 7. API > Retunerar ett resultat
// 8. UI > Visar feedback


// UI > Pages
// API > Endpoints: POST, GET, PUT, DELETE, api/ai/feedback
// BLL: logik, rätt, fel, progression, services
// DAL : migration, dbContext, modeller(endast för DATABASE)
// Shhared: DTO objekt som används mellan lager