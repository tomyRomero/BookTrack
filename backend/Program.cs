using System.Text;
using backend.Data_Access;
using backend.Models;
using backend.Repositories;
using backend.Repositories.Interfaces;
using backend.Services;
using backend.Services.Interfaces;
using backend.Utilities;
using backend.Utilities.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//configure JWT authentication settings.
//binds the "JwtSettings" section from the appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Add services to the container.
builder.Services.AddControllers();

// Add authentication and specify the default scheme
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Directly retrieving values from appsettings.json
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");

    // Check if the Secret value is null or empty
    var secretKey = jwtSettings["Secret"];
    if (string.IsNullOrEmpty(secretKey))
    {
        throw new InvalidOperationException("JWT Secret key is not configured properly in appsettings.json");
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,  // Not validating the issuer
        ValidateAudience = false, // Not validating the audience
        ValidateLifetime = true,  // Still validate the expiration time of the token
        ValidateIssuerSigningKey = true,  // Always validate the signing key
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]))  
    };
});

//Configure Cross-Origin Resource Sharing
builder.Services.AddCors(co =>
{
    co.AddPolicy("CORS", pb =>
    {
        // Allows any origin to access the backend.
        pb.WithOrigins("*")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

//define how JSON serialization should be configured
builder.Services.AddControllers()
.AddJsonOptions(options =>
    {
        //Ignore cycles, ensures I don't get an infinite loop when serializing objects with circular references, 
        // such as entity models with navigation properties
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register dependencies

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();

builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();

builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IHasher, Hasher>();

// Configure application's DbContext to use SQL Server.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure logging to the console.
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CORS");

//Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

//authorization middleware.
app.UseAuthorization();

// Map API controllers to handle incoming HTTP requests.
app.MapControllers();

app.Run();
