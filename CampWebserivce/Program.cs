using CampLib.Repository;
using Microsoft.EntityFrameworkCore;
using KlasseLib;
using WebApplication1;
using CampWebservice.Configuration;
using CampWebservice.Services;

var builder = WebApplication.CreateBuilder(args);

// =======================
// DATABASE
// =======================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// =======================
// REPOSITORIES
// =======================
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddSingleton<StaffRepository>();

// =======================
// CLOUDINARY
// =======================
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary")
);
builder.Services.AddSingleton<CloudinaryService>();

// =======================
// SESSION (VIGTIG)
// =======================
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".CampFeed.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// =======================
// CONTROLLERS + SWAGGER
// =======================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =======================
// CORS
// =======================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// =======================
// MIDDLEWARE PIPELINE
// =======================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS skal ligge tidligt
app.UseCors("AllowAll");

// app.UseHttpsRedirection(); // valgfri i dev

app.UseRouting();

// 🔥 SESSION SKAL LIGGE HER
app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();
