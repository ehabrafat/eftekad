using System.Reflection;
using System.Text;
using Eftekad.Config;
using Eftekad.Data;
using Eftekad.Endpoints;
using Eftekad.Shared.Abstractions;
using Eftekad.Shared.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EfDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpoints();
builder.Services.AddAutoMapper(
    cfg => { /* optional configuration */ },
    AppDomain.CurrentDomain.GetAssemblies()  // This is a params Assembly[] parameter
);

builder.Services.AddScoped<ICurrentUser, CurrentUser>();
var jwt = builder.Configuration.GetRequiredSection("Jwt").Get<JwtConfig>();
builder.Services.AddSingleton(jwt!);
var key = Encoding.UTF8.GetBytes(jwt!.Secret);

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddAuthentication()
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero // Remove delay on token expiration
        };
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Eftekad",
        Version = "v1",
    });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UsePathBase("/api");
app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();

app.Run();

