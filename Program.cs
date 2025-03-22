using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using EyeOfHorusP.Application.Interfaces.Repositories;
using EyeOfHorusP.Application.Interfaces;
using EyeOfHorusP.Application.Services;
using EyeOfHorusP.Domain.Entities;
using EyeOfHorusP.Infrastructure.Mapping;
using EyeOfHorusP.Infrastructure.Persistence.Repositories;
using EyeOfHorusP.Infrastructure.Persistence;
using EyeOfHorusP.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// ✅ تحميل إعدادات JSON
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// ✅ إضافة الخدمات الأساسية
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// ✅ تفعيل CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// ✅ إعداد قاعدة البيانات
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(); // ⚠️ مفيد أثناء التطوير فقط
});

// ✅ إعداد الهوية (Identity)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ✅ إعداد AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

// ✅ تسجيل الخدمات
builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

// ✅ تسجيل المستودعات (Repositories)
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ✅ إعداد Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer {your token}' below.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

// ✅ إعداد التحقق من المفتاح السري (Secret Key)
string secretKey = builder.Configuration["ApiSettings:Secret"];
if (string.IsNullOrWhiteSpace(secretKey))
{
    throw new Exception("❌ Secret Key is missing from appsettings.json!");
}

// ✅ إعداد المصادقة باستخدام JWT
var key = Encoding.ASCII.GetBytes(secretKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.FromDays(7)
    };
});

// ✅ تسجيل المعالج العام للأخطاء
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// 🔥 **بناء التطبيق**
var app = builder.Build();

// ✅ تنفيذ المايجريشن فقط إذا كانت هناك تحديثات جديدة
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
    }

    // ✅ إضافة الأدوار عند تشغيل التطبيق
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// ✅ تهيئة الـ Swagger أثناء التطوير
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
    });
}

// ✅ تفعيل Middleware الخاصة بالأخطاء
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
