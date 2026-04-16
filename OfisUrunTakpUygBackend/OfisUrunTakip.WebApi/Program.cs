using Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using OfficeOpenXml;
using OfisUrunTakip.WebApi.Data;
using OfisUrunTakip.WebApi.Entity;
using OfisUrunTakip.WebApi.Services;
using Repositories;
using Repositories.Contracts;
using System.Text;


var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Uygulama başlatılıyor (init main)...");

try
{
    
    var builder = WebApplication.CreateBuilder(args);

    
    builder.Logging.ClearProviders(); // Varsayılan logları temizle
    builder.Host.UseNLog();           // NLog'u kullan
  

   
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

    
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            policy =>
            {
                policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
    });

    
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,

                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            };
        });

  
    builder.Services.AddDbContext<ApiContext>();
    builder.Services.AddAutoMapper(typeof(MappingProfile));

   
    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<IEmailNotificationRepository, EmailNotificationRepository>();
    builder.Services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();
    builder.Services.AddScoped<IEmailNotificationSettingRepository, EmailNotificationSettingRepository>();

    builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
    builder.Services.AddScoped<EmailReportService>();

    builder.Services.AddHostedService<OfisUrunTakip.WebApi.Workers.LowStockMailWorker>();//mail gondermek ıcın workers klasoru ıcın 

    
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme."
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }});
    });

  
    var app = builder.Build();

   

    app.UseCors("AllowAll");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    //  Uygulama çökerse logla
    logger.Error(exception, "Program beklenmedik bir hata yüzünden durduruldu!");
    throw;
}
finally
{
    // Logları diske yaz ve kapat
    NLog.LogManager.Shutdown();
}