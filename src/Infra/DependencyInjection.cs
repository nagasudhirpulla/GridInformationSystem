using App.Common.Interfaces;
using App.Config;
using App.MeasurementData.Interfaces;
using Ardalis.GuardClauses;
using Core.Constants;
using Infra.Data;
using Infra.Data.Interceptors;
using Infra.Data.LocalMeasDataStores;
using Infra.Identity;
using Infra.Identity.TokenProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

#if (UseSQLite)
            options.UseSqlite(connectionString);
#else
            options.UseNpgsql(connectionString);
#endif
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddScoped<IMeasDataStore, SqliteMeasDataStore>();
        services.AddScoped<IProxyDataSourceFetcher, ProxyDataSourceFetcher>();

#if (UseApiOnly)
        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();
#else
        services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 4;
            options.Password.RequiredUniqueChars = 2;
            options.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
        })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddTokenProvider<EmailConfirmationTokenProvider<ApplicationUser>>("emailconfirmation");

        // set email confirmation token lifespan to 3 days
        services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
            opt.TokenLifespan = TimeSpan.FromDays(3));

        //services.ConfigureApplicationCookie(options =>
        //{
        //    // configure login path for return urls
        //    // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio
        //    options.LoginPath = "/Identity/Account/Login";
        //    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        //});
#endif

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthorizationBuilder()
            .AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator));

        return services;
    }

    public static IServiceCollection AddDataApiInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ReadOnlyAppDbContext>((sp, options) =>
        {
#if (UseSQLite)
            options.UseSqlite(connectionString);
#else
            options.UseNpgsql(connectionString);
#endif
        });

        // Adding Authentication
        // add jwt config
        JwtConfig jwtConfig = configuration.GetSection("JWT").Get<JwtConfig>() ?? throw new Exception("JWT section not present in config");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = jwtConfig.ValidAudience,
                ValidIssuer = jwtConfig.ValidIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret))
            };
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ReadOnlyAppDbContext>());
        services.AddScoped<IMeasDataStore, SqliteMeasDataStore>();
        services.AddScoped<IProxyDataSourceFetcher, ProxyDataSourceFetcher>();

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, ApiIdentityService>();

        return services;
    }
}
