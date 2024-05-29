using ChatAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpsRedirection(options => options.HttpsPort =7009);

builder.Services.AddDbContext<ChatDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddIdentity<User, IdentityRole>(options => 
options.SignIn.RequireConfirmedEmail = false


)//identityRole yetkilendirme iþlemleri için gerekli
    .AddEntityFrameworkStores<ChatDbContext>()
    .AddDefaultTokenProviders();


builder.Services.Configure<TokenOption>(builder.Configuration.GetSection("TokenOption"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,options =>
    {
        TokenOption tokenOption = builder.Configuration.GetSection("TokenOption").Get<TokenOption>();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = tokenOption.Issuer,
            ValidAudience = tokenOption.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOption.SecretKey))
        };
    });



builder.Services.AddCors(options =>
{
    options.AddPolicy("ChatApp", builder =>
    {
        builder.WithOrigins("https://localhost:7087", "http://localhost:5171")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRequestLocalization();

app.UseHttpsRedirection();

app.UseCors("ChatApp");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
