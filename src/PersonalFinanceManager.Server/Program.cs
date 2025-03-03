using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalFinanceManager.Server.Data;
using PersonalFinanceManager.Server.Repositories;
using PersonalFinanceManager.Server.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// configure JWT settings
var jwtKey = builder.Configuration["Jwt:SecretKey"];
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
// Add JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = key
        };
    });
builder.Services.AddAuthorization();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework Core with PostgreSQL
builder.Services.AddDbContext<FinanceManagerDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register dependencies
builder.Services.AddSingleton<JwtService>();
builder.Services.AddScoped<UserRepository>();

// Register FluentMigrator
builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddPostgres()
        .WithGlobalConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"))
        .ScanIn(typeof(Program).Assembly).For.Migrations());


var app = builder.Build();

// Run migrations automatically at startup
using (var scope = app.Services.CreateScope())
{ 
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable authentication and authorization in the request pipeline
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
