using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QRefeicao.Identity;
using QRefeicao.Identity.Initializer;
using QRefeicao.Identity.Models;
using QRefeicao.Identity.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var supportedCultures = new[] { "pt" };

var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);


builder.Services.AddDbContextPool<AuthContext>(options =>
{
    options.UseNpgsql(Environment.GetEnvironmentVariable("QRConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<LocalizedIdentityErrorDescriber>();


var jwtSecret = builder.Configuration.GetSection("JwtSettings:Secret");
var issuer = builder.Configuration.GetSection("JwtSettings:Issuer");
var audience = builder.Configuration.GetSection("JwtSettings:Audience");

if (string.IsNullOrEmpty(jwtSecret.Value))
{
    throw new InvalidOperationException("JWT SECRET IS NOT SET");
}
if (string.IsNullOrEmpty(issuer.Value))
{
    throw new InvalidOperationException("JWT ISSUER IS NOT SET");
}
if (string.IsNullOrEmpty(audience.Value))
{
    throw new InvalidOperationException("JWT AUDIENCE IS NOT SET");
}

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto |
        ForwardedHeaders.XForwardedHost;
});

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

        ValidIssuer = issuer.Value,
        ValidAudience = audience.Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret.Value))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.TryGetValue("AuthToken", out var token))
            {
                context.Token = token;

            }
            return Task.CompletedTask;
        }
    };
});


if (builder.Environment.IsProduction())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
        {
            builder.WithOrigins("https://danieloliveira.net.br", "https://www.danieloliveira.net.br")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
    });

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => {
            options.Cookie.Name = "AuthToken";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Lax; // or lax
            options.ExpireTimeSpan = TimeSpan.FromHours(3);
        });

}
else
{

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.Cookie.Name = "AuthToken";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        options.Cookie.SameSite = SameSiteMode.None; // or Strict
        options.ExpireTimeSpan = TimeSpan.FromHours(3);
    });
}

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;

});

builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.AddControllers();

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Digite 'Bearer' [espaço] e seu token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference=new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header
       },
        new List<string>()
    }
    });
});

builder.Services.AddScoped<IDBInitializer, DBInitializer>();
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "X-CSRF-TOKEN";
});

var app = builder.Build();

app.UseForwardedHeaders();

app.UseMiddleware<CustomMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(cors =>
    {
        cors.AllowAnyHeader();
        cors.AllowAnyMethod();
        cors.WithOrigins("http://localhost:5173", "https://localhost:44352");
        cors.AllowCredentials();
    });
}
else
{
    app.UseCors("AllowAll");
}
using var scope = app.Services.CreateScope();
var initializer = scope.ServiceProvider.GetRequiredService<IDBInitializer>();
initializer.Initialize();

/*
app.Use((context, next) =>
{
    var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();
    var tokens = antiforgery.GetAndStoreTokens(context);

    // Set HttpOnly = false so JavaScript can read the cookie to send it back in a header
    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!,
        new CookieOptions { HttpOnly = false, Path = "/" });

    return next(context);
});

app.UseAntiforgery();
*/
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
