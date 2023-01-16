using Microsoft.EntityFrameworkCore;
using TestWebApiOnlineMove.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql("Host=localhost;Port=5432;Database=TestDataBaseOnline;Username=postgres;Password=16031003"));
builder.Services.AddCors(optinos => optinos.AddPolicy("CorsPolicy",
    bulder =>
    {
        bulder.WithOrigins("http://localhost:3000", "http://192.168.0.105:3000", "http://192.168.0.101:3000")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();

    }));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
