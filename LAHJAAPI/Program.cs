using Api;
using ApiCore.Schedulers;
using AutoGenerator;
using LAHJAAPI.CustomPolicy;
using AutoGenerator.Notifications.Config;
using AutoGenerator.Schedulers;
using LAHJAAPI.Data;
using LAHJAAPI.Models;
using LAHJAAPI.Utilities;
using LAHJAAPI.V1.Validators.Conditions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);



#region External Services
//Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
//    .WriteTo.Console()
//    .WriteTo.File("logs/myLog-.txt", rollingInterval: RollingInterval.Day)
//    .CreateLogger();
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services
    .AddStripeGateway(builder.Configuration)
    .AddDataContext(builder.Configuration)
    ;
#endregion

builder.Services.
    AddAutoBuilderApiCore(new()
    {

        Arags = args,
        NameRootApi = "V1",
        IsMapper = true,
        TypeContext = typeof(DataContext),
        Assembly = Assembly.GetExecutingAssembly(),
        AssemblyModels = typeof(LAHJAAPI.Models.Advertisement).Assembly,
        DbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection"),

    })
    .AddAutoValidator()
    .AddAutoConfigScheduler()

    .AddAutoNotifier(new()
    {

        MailConfiguration = new MailConfig()
        {
            SmtpUsername = "gamal333ge@gmail.com",
            SmtpPassword = "bxed hnwv vqlt ddwy",
            SmtpHost = "smtp.gmail.com",
            SmtpPort = 587,
            FromEmail = "gamal333ge@gmail.com",
            NameApp = "LAHJA-API" // عيّن اسم التطبيق هنا كما يناسبك

        }


    });

//TODO: preapare the shared
//TODO: preapare the services
//TODO: preapare the controller
//TODO: read about Mock I see it in deepseek => repositories


// Add services to the container.

builder.Services.AddProblemDetails();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.RespectRequiredConstructorParameters = true;
    });
builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Debug);

// Scoped Services
builder.Services.AddApiServices(builder.Configuration);



//builder.Services.AddTransient<IClaimsTransformation, MyClaimsTransformation>();

var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

// تمكين عرض الأخطاء المتعلقة بـ JWT
IdentityModelEventSource.ShowPII = true;

builder.Services.AddDynamicAuthentication(builder.Configuration, appSettings);

// Configure authorization
builder.Services.AddAuthorizationBuilder();


// Add identity and opt-in to endpoints
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders()
    //.AddUserConfirmation<ApplicationUser>()
    .AddApiEndpoints();

builder.Services.Configure<IdentityOptions>(options =>
{
    /*
     *  يسمح للمستخدمين الجدد بتجربة تسجيل الدخول دون القلق من حظر حساباتهم.
     */
    options.Lockout.AllowedForNewUsers = true;
});
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanViewPlan", policy =>
        policy.Requirements.Add(new PermissionRequirement("Permission", Permissions.ViewPlan))
        ); // Example: Requires a claim
});

// Add a CORS policy for the client
builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy.WithOrigins([builder.Configuration["appsettings:baseurls:api"] ?? "https://localhost:7001",
            builder.Configuration["FrontendUrl"] ?? "https://localhost:7003"])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{
    //c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert token with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
            },
            Array.Empty<string>()
        }
    });
    // تخصيص Swagger لتحديد القيم
    //c.OperationFilter<CustomProductParameterFilter>();
});

//builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddLogging(); // ÅÖÇÝÉ ÎÏãÇÊ ÇáÜ Logging

//builder.Services.AddScoped<IInvoiceShareRepository, InvoiceShareRepository>();
//builder.Services.AddScoped<InvoiceService>();

var app = builder.Build();
app.UseSchedulerDashboard();

//SeedData.EnsureSeedData(app);

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        //o.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        //o.RoutePrefix = string.Empty;
        // collapse endpoints 
        o.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);

    });


app.CustomMapIdentityApi<ApplicationUser>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();

// protection from cross-site request forgery (CSRF/XSRF) attacks with empty body
// form can't post anything useful so the body is null, the JSON call can pass
// an empty object {} but doesn't allow cross-site due to CORS.
app.MapPost("/api/logout", async (
    SignInManager<ApplicationUser> signInManager,
    [FromBody] object empty) =>
{
    if (empty is not null)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
    return Results.NotFound();
}).RequireAuthorization().WithTags("Auth");

app.Run();
