using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Diagnostics;
using TestWebApiOnlineMove.Common;
using TestWebApiOnlineMove.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var authOption = builder.Configuration.GetSection("Auth");

Debug.WriteLine(authOption);

builder.Services.Configure<AuthOptions>(authOption);

//builder.Services.AddCors(option =>
//{
//    option.AddDefaultPolicy(
//        builder =>
//        {
//            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//        });
//});


builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
    builder =>
    {
        builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    }));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.RequireHttpsMetadata = false; // ��������� ����������� ����� �� http � �� �� https
        option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true, // �������� �������� ������
            ValidIssuer = authOption.Get<AuthOptions>().Issuer,

            ValidateAudience = true, // �������� ���������� ������
            ValidAudience = authOption.Get<AuthOptions>().Audience,

            ValidateLifetime = true, 

            IssuerSigningKey = authOption.Get<AuthOptions>().GetSymmetricSecurityKey(), //HS256
            ValidateIssuerSigningKey = true,
        };
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql("Host=localhost;Port=5432;Database=TestDataBaseOnline;Username=postgres;Password=16031003"));




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
