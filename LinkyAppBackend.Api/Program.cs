using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sieve.Models;
using LinkyAppBackend.Api.AutoMapper;
using LinkyAppBackend.Api.Filters;
using LinkyAppBackend.Api.Handlers;
using LinkyAppBackend.Api.Handlers.Interfaces;
using LinkyAppBackend.Api.Helpers;
using LinkyAppBackend.Api.Helpers.Interfaces;
using LinkyAppBackend.Api.Middleware;
using LinkyAppBackend.Api.Models.Entities;
using LinkyAppBackend.Api.Models.Entities.Master;
using LinkyAppBackend.Api.Models.Options;
using LinkyAppBackend.Api.Providers;
using LinkyAppBackend.Api.Services;
using LinkyAppBackend.Api.Services.Interfaces;
using LinkyAppBackend.Api.Sieve;
using Sieve.Services;
using GoogleOptions = LinkyAppBackend.Api.Models.Options.SocialAuth.GoogleOptions;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.Configure<SieveOptions>(builder.Configuration.GetSection("Sieve"));
builder.Services.AddScoped<IPagingHelper, PagingHelper>();

builder.Services.AddScoped<IJwtHandler, JwtHandler>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthContextProvider, AuthContextProvider>();
builder.Services.AddScoped<ErrorHandlingMiddleWare>();

builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<DtoMappingProfile>(); });
builder.Services.AddScoped<ISieveProcessor, AppSieveProcessor>();

#endregion

#region Database

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

#endregion

#region Identity

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(o =>
{
    o.Password.RequireDigit = true;
    o.Password.RequireLowercase = true;
    o.Password.RequireNonAlphanumeric = true;
    o.Password.RequireUppercase = true;
    o.Password.RequiredLength = 6;
    o.Password.RequiredUniqueChars = 1;
    o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    o.Lockout.MaxFailedAccessAttempts = 5;
    o.Lockout.AllowedForNewUsers = true;
    o.User.RequireUniqueEmail = true;
});

#endregion

#region JWT Authentication and Authorization

var jwtOptionsSection = builder.Configuration.GetSection("JwtOptions");
var jwtOptions = jwtOptionsSection.Get<JwtOptions>();
builder.Services.Configure<JwtOptions>(jwtOptionsSection);

var googleOptionsSection = builder.Configuration.GetSection("SocialAuth:Google");
builder.Services.Configure<GoogleOptions>(googleOptionsSection);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwtOptions!.Issuer,
            ValidAudience = jwtOptions!.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.Key)),

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorizationBuilder()
    .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser().Build());

#endregion

#region Config

builder.Services.AddHttpContextAccessor();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => { options.SuppressAsyncSuffixInActionNames = false; });
builder.Services.AddSwaggerGen(option =>
    {
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.OperationFilter<AuthResponseOperationFilter>();
        option.OrderActionsBy(apiDesc => apiDesc.RelativePath);
    }
);

builder.Services.AddCors(
    options =>
    {
        options.AddPolicy("AllowAll",
            policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
    });

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "api/swagger/{documentname}/swagger.json";
    });
    app.UseSwaggerUI(c =>
    {
        c.EnablePersistAuthorization();
        c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "v1");
        c.RoutePrefix = "api/swagger";
    });
    app.UseCors("AllowAll");

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.MapControllers();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleWare>();

app.Run();