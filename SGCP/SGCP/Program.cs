using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using SGCP.Mappings;
using SGCP.Models;
using SGCP.Security;
using SGCP.Security.Authorization;
using SGCP.Security.Configuration;
using SGCP.Services;
using SGCP.Services.Logger;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // solo si usás cookies o autenticación con credenciales
    });
});

// Lee la configuración de Serilog desde appsettings.json
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDbContext<SGCP_DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

// Add services to the container.
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICatalogsService, CatalogsService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IComponentService, ComponentService>();
builder.Services.AddScoped<IProductComponentService, ProductComponentService>();
builder.Services.AddScoped<ILaborCostService, LaborCostService>();
builder.Services.AddScoped<IRegionalsPriceService, RegionalsPriceService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IInventoryMovementService, InventoryMovementService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<ILaborTypeService, LaborTypeService>();
builder.Services.AddScoped<IComponentBuilderService, ComponentBuilderService>();

builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddHostedService<TokenCleanupService>();


builder.Services.Configure<TokenCleanupSettings>(
    builder.Configuration.GetSection("TokenCleanup"));

builder.Services.AddHostedService<TokenCleanupService>();
// CARGA DINÁMICA DE PERMISOS
List<string> permissionList;

using (var tempProvider = builder.Services.BuildServiceProvider())
{
    using var scope = tempProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<SGCP_DbContext>();

    permissionList = context.Permissions
        .Where(p => p.Enable)
        .Select(p => p.Name)
        .Distinct()
        .ToList();
}
// REGISTRAR LAS POLÍTICAS
builder.Services.AddAuthorization(options =>
{
    foreach (var permission in permissionList)
    {
        options.AddPolicy($"Permission:{permission}", policy =>
            policy.Requirements.Add(new PermissionRequirement(permission)));
    }
});

// Para AddJwtBearer
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

// Para que funcione IOptions<JwtSettings>
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    }); ;


builder.Services.AddScoped(typeof(ILoggingService<>), typeof(LoggingService<>));

builder.Services.AddAutoMapper(typeof(ProductProfile));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SGCP API", Version = "v1" });

    // 🔐 Configuración para soportar JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();



app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var mapperConfig = scope.ServiceProvider.GetRequiredService<IMapper>().ConfigurationProvider;
    mapperConfig.AssertConfigurationIsValid(); // 💥 Lanza excepción si falta un mapa

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();
