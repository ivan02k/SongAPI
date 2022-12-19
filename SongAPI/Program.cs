using Data;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Service.CSV;
using SongAPI.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SongContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICSVService, CSVService>();
builder.Services.AddTransient<CSVService>();
builder.Services.AddTransient<ServiceCaller>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var service=app.Services.GetRequiredService<ServiceCaller>();
service.ReadData();

app.Run();


