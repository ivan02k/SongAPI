
using Data;
using Data.Entities;
using Data.Entities.IdentityClass;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using Service;
using Service.CSV;
using SongAPI.Common;
using ViewModels.Data.ViewModels;
using Mapper;
using Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SongContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddRoles<ApplicationRole>()
.AddEntityFrameworkStores<SongContext>();

builder.Services.AddTransient<IUserService, UserService>();

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfile());
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddTransient<IDataService, DataService>();
builder.Services.AddTransient<IPDFService, PDFService>();

builder.Services.AddTransient<IBaseService<ArtistViewModel>, ArtistService>();
builder.Services.AddTransient<IBaseService<SongViewModel>, SongService>();
builder.Services.AddTransient<IBaseService<SongSpecificationViewModel>, SongSpecificationService>();

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

app.UseAuthentication();
app.UseAuthorization();

var scope = app.Services.CreateScope();
var service = scope.ServiceProvider.GetService<ServiceCaller>();
if(service != null) service.ReadData();

app.Run();


