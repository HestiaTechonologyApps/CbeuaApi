using Cbeua.Bussiness;
using Cbeua.Domain.Configurations;
using Cbeua.InfraCore;
using Cbeua.InfraCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;
using System.Text;



var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration as IConfiguration)
    .CreateLogger(); 
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// JWT Authentication configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Cbeua.Api.Security.AuthPolicies.AdminOnly,
        policy => policy.RequireAuthenticatedUser()
                        .RequireAssertion(ctx => Cbeua.Api.Security.ClaimsPrincipalExtensions.IsAdmin(ctx.User)));
});
builder.Services.AddHttpContextAccessor();
CleanOldStdoutLogs();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddSwaggerGen(c =>
//{
//    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//    c.IncludeXmlComments(xmlPath);
//});


builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Add JWT Bearer Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddMemoryCache();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<OtpSettings>(builder.Configuration.GetSection("OtpSettings"));
builder.Services.Configure<WalletSettings>(builder.Configuration.GetSection("Wallet"));

// Register AppDbContext with SQL Server (adjust as needed)
builder.Services.AddInfraCoreServiceCollectionExtensions(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

var wellKnownPath = Path.Combine(Directory.GetCurrentDirectory(), ".well-known");

// Create the directory if it doesn't exist
if (!Directory.Exists(wellKnownPath))
{
    Directory.CreateDirectory(wellKnownPath);
}

// Bank contribution files hold member names and staff numbers; they are read by the API, never served directly.
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/contributionfiles", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        return;
    }
    await next();
});

// ADD THIS LINE - Serves files from wwwroot (including /profilepics)
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx => ctx.Context.Response.Headers["X-Content-Type-Options"] = "nosniff"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(wellKnownPath),
    RequestPath = "/.well-known",
    ServeUnknownFileTypes = true
});
app.UseMiddleware<ExceptionLoggingMiddleware>();

// Swagger publishes the full API map, so it is off in production unless "EnableSwagger": true is configured.
if (app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("EnableSwagger"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors();
// Use authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
void CleanOldStdoutLogs()
{
    try
    {
        var logFolder = Path.Combine(Directory.GetCurrentDirectory(), "logs");
        if (!Directory.Exists(logFolder))
            return;

        var logFiles = Directory.GetFiles(logFolder, "stdout*");

        foreach (var file in logFiles)
        {
            try
            {
                var info = new FileInfo(file);
                // Delete if older than 7 days
                if (info.CreationTime < DateTime.Now.AddDays(-7))
                {
                    info.Delete();
                }
            }
            catch
            {
                // ignore errors if file is in use
            }
        }
    }
    catch (Exception)
    {

        throw;
    }
   
}
