using DataBundle.BundleService;
using DataBundle.DataAccess;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataBundle;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddMvc();

//cookie is provided to cllient to maintain the session state. the cookie contains the sessionID
builder.Services.Configure<CookiePolicyOptions>(
    options =>
    {
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
    }
 );

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = true;
});

//configure swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TelegramBot", Version = "v1" });

    options.AddSecurityDefinition("token" , new OpenApiSecurityScheme{
        Name= "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer", 
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JSON Web Token based security"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "token"
                },
            },
            Array.Empty<string>()
        }
    });
});



builder.Services.AddSingleton<IDataAccess, DataAccess>();
builder.Services.AddSingleton<ITelegramBotService, TelegramBotService>();
builder.Services.AddSingleton<IJWTBearerConfiguration, JWTBearerConfiguration>();
builder.Services.AddSingleton<IOTPManager, OTPManager>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization(options => options.FallbackPolicy = new AuthorizationPolicyBuilder().
RequireAuthenticatedUser().AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).Build()
);

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();


app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
