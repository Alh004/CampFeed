using CampLib.Repository;
using Microsoft.EntityFrameworkCore;
using KlasseLib;
using WebApplication1;
using CampWebservice.Configuration;
using CampWebservice.Services;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")) // connection string
);

// Repos
builder.Services.AddScoped<CategoryRepository>(); // pr. request

// Cloudinary
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary") // læs config
);
builder.Services.AddSingleton<CloudinaryService>(); // én instans


builder.Services.AddScoped<CampWebservice.Services.IEmailService, CampWebservice.Services.EmailService>();
builder.Services.AddScoped<IEmailService, EmailService>();



// API + Swagger
builder.Services.AddControllers();           // controllers
builder.Services.AddEndpointsApiExplorer();  // swagger endpoints
builder.Services.AddSwaggerGen();            // swagger docs

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // alle origins (dev)
            .AllowAnyMethod() // alle methods
            .AllowAnyHeader();// alle headers
    });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   // swagger json
    app.UseSwaggerUI(); // swagger UI
}

app.UseCors("AllowAll"); // tillad frontend

// app.UseHttpsRedirection(); // https redirect (valgfri)

app.UseAuthorization();  // auth pipeline
app.MapControllers();    // map routes
app.Run();               // start app