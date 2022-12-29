using System.Text;
using Backend.Api.Filters;
using Backend.Api.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// SETUP CORS
const string corsPolicyName = "corsPolicyName";
builder.Services
    .AddCors(options =>
    {
        options.AddPolicy(name: corsPolicyName, policy =>
        {
            policy
                .WithOrigins("http://localhost", "https://localhost")
                .WithMethods("OPTIONS", "GET", "POST")
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(_ => true);
        });
    });

// SETUP AUTHENTICATION
var secret = builder.Configuration.GetValue<string>("JwtSecret");
var key = Encoding.ASCII.GetBytes(secret!);
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// SETUP SIGNALR SERVICES
builder.Services
    .AddSignalR()
    .AddAzureSignalR();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var securitySchema = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer Token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = JwtBearerDefaults.AuthenticationScheme,
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.DocumentFilter<HubNegotiationDocumentFilter>();
    options.AddSecurityDefinition(securitySchema.Reference.Id, securitySchema);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement() { { securitySchema, Array.Empty<string>() } });
});

var app = builder.Build();

app.UseSwagger().UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<MessagingHub>("/messaging");

app.MapControllers();

app.Run();
