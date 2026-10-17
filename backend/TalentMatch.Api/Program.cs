using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TalentMatch.Api.Extensions;
using TalentMatch.Application.Interfaces;
using TalentMatch.Application.Dtos;
using TalentMatch.Application.Services;
using TalentMatch.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Configura ConnectionStrings:DefaultConnection con las credenciales de MySQL.");
}
var bootstrapAdminUserName = builder.Configuration["BootstrapAdmin:UserName"];
var bootstrapAdminPassword = builder.Configuration["BootstrapAdmin:Password"];
if (string.IsNullOrWhiteSpace(bootstrapAdminUserName) != string.IsNullOrWhiteSpace(bootstrapAdminPassword))
{
    throw new InvalidOperationException(
        "Configura tanto BootstrapAdmin:UserName como BootstrapAdmin:Password, o deja ambos vacíos.");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TalentMatch API",
        Version = "v1",
        Description = "API para autenticación y autorización de TalentMatch."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT con el formato: Bearer {token}"
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, MySqlUserRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"] ?? "SuperSecretKeyForTalentMatchProject1234567890";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "TalentMatch",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "TalentMatchUsers",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOrAdmin", policy => policy.RequireRole("User", "Admin"));
});

builder.Services.AddProblemDetails();

var app = builder.Build();

if (!string.IsNullOrWhiteSpace(bootstrapAdminUserName) &&
    !string.IsNullOrWhiteSpace(bootstrapAdminPassword))
{
    await using var scope = app.Services.CreateAsyncScope();
    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    var existingAdmin = await userRepository.GetByUserNameAsync(bootstrapAdminUserName);
    if (existingAdmin is null)
    {
        await userRepository.CreateAsync(new UserCredentialsDto(
            bootstrapAdminUserName,
            PasswordHashing.Create(bootstrapAdminPassword),
            "Admin"));
    }
}

if (app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TalentMatch API v1"));
}

app.UseGlobalExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
