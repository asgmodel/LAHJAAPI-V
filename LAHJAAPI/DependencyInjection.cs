using Api.Config;
using Api.LAHJAAPI.Utilities;
using APILAHJA.LAHJAAPI.Utilities;
using AutoGenerator.Config;
using LAHJAAPI.Services2;
using LAHJAAPI.Models;
using LAHJAAPI.Utilities;
using LAHJAAPI.V1.Services.Statistics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using LAHJAAPI.Utilities.Services2;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.AddHttpContextAccessor();
        services.AddHttpClient();  // Register HttpClient for dependency injection
        services.AddMemoryCache();
        services.AddAutoMapper(typeof(StripeMappingConfig));
        services.AddAutoMapper(typeof(MappingConfig));
        services.AddSingleton<IEmailService, EmailService>();
        services.AddSingleton<IEmailSender<ApplicationUser>, EmailService>();
        services.AddScoped<IUserClaimsHelper, UserClaimsHelper>();
        services.AddScoped<SubscriptionCheckFilter>();

        // options
        services.Configure<AppSettings>(configuration.GetSection("appsettings"));
        services.Configure<SmtpConfig>(configuration.GetSection(nameof(SmtpConfig)));

        services.AddSingleton<ClaimsChange>();
        services.AddScoped<TokenService>();

        services.AddScoped<IStatisticsService, StatisticsService>();

        //services.AddScoped<UserService>();
        //services.AddHostedService<TrackSubscriptionLoader>();

        // // Scoped Repositories Automatically
        // services.Scan(scan => scan
        //.FromAssemblyOf<IAppUserShareRepository>() // Adjust to your assembly
        //.AddClasses(classes => classes.Where(c => c.Name.EndsWith("ShareRepository")))
        //.AsImplementedInterfaces()
        //.WithScopedLifetime());


        // ////Services
        // services.Scan(scan => scan
        // .FromAssemblyOf<BaseService>() // Adjust to your assembly
        // .AddClasses(classes => classes.AssignableTo<BaseService>())
        // //.AddClasses(classes => classes.Where(c => c.Name.EndsWith("Service")))
        // .AsImplementedInterfaces()
        // .WithScopedLifetime());


        // Behaviors
        //services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        // Filters
    }

    public static void AddDefaultAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(op =>
        {
            op.DefaultScheme = IdentityConstants.BearerScheme;
            op.DefaultChallengeScheme = IdentityConstants.BearerScheme;
        })

    .AddBearerToken(IdentityConstants.BearerScheme)
    .AddIdentityCookies();
    }

    public static void AddDynamicAuthentication(this IServiceCollection services, IConfiguration configuration, AppSettings appSettings)
    {
        services.AddAuthentication(op =>
        {
            op.DefaultAuthenticateScheme = "DynamicScheme";
            op.DefaultChallengeScheme = "DynamicScheme";
            op.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
            .AddPolicyScheme("DynamicScheme", "Dynamic Authentication Scheme", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    // التحقق مما إذا كان الطلب يحتوي على Bearer Token
                    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                    {
                        var token = authHeader["Bearer ".Length..];
                        if (token.Split('.').Length == 3)  // JWT يحتوي دائمًا على 3 أجزاء
                            return JwtBearerDefaults.AuthenticationScheme;
                        return IdentityConstants.BearerScheme; // استخدم Bearer Token
                    }

                    return IdentityConstants.ApplicationScheme; // استخدم الكوكيز
                };
            })
            .AddBearerToken(IdentityConstants.BearerScheme, o =>
            {
                o.BearerTokenExpiration = TimeSpan.FromDays(10);
            })
             .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = appSettings.Jwt.validIssuer,
                     ValidAudience = appSettings.Jwt.ValidAudience,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Jwt.Secret))
                 };
                 // السماح باستخدام التوكن في الكوكيز
                 options.Events = new JwtBearerEvents
                 {
                     OnMessageReceived = context =>
                     {
                         if (context.Request.Cookies.ContainsKey("access_token"))
                         {
                             context.Token = context.Request.Cookies["access_token"];
                         }
                         return Task.CompletedTask;
                     }
                 };
             })
              .AddGoogle(opt =>
              {
                  var googleAuth = configuration.GetSection("Authentication:Google");
                  opt.ClientId = googleAuth["ClientId"];
                  opt.ClientSecret = googleAuth["ClientSecret"];
                  opt.SignInScheme = IdentityConstants.ExternalScheme;
                  opt.CallbackPath = "/signin-google"; // مسار التوجيه بعد تسجيل الدخول
                  opt.SaveTokens = true;
                  //opt.Events.OnCreatingTicket = ctx =>
                  //{
                  //    List<AuthenticationToken> tokens = ctx.Properties.GetTokens().ToList();

                  //    tokens.Add(new AuthenticationToken()
                  //    {
                  //        Name = "tokenTicket",
                  //        Value = DateTime.UtcNow.ToString()
                  //    });

                  //    ctx.Properties.StoreTokens(tokens);

                  //    return Task.CompletedTask;
                  //};
              })
            .AddIdentityCookies()
            ;
    }

    public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string[] permissions)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        foreach (var permission in permissions)
        {
            if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }

    public static string GetDisplayName(this Enum enumValue)
    {
        var enumType = enumValue.GetType();
        var memberInfo = enumType.GetMember(enumValue.ToString());
        var displayAttribute = memberInfo[0].GetCustomAttribute<DisplayAttribute>();

        return displayAttribute?.Name ?? enumValue.ToString();
    }
}
