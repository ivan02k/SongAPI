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
using Authorization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Interfaces;
using Service.Cached;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
             new OpenApiSecurityScheme
             {
               Reference = new OpenApiReference
               {
                 Id = "Bearer",
                Type = ReferenceType.SecurityScheme
               }
             },
             new string[] {}
        }
    });
});

//JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Secret"])),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddDbContext<SongContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<DataSeeder>();

//builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
//{
//    options.SignIn.RequireConfirmedAccount = true;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireLowercase = false;
//})
//.AddRoles<ApplicationRole>()
//.AddEntityFrameworkStores<SongContext>();


builder.Services.AddCors();
builder.Services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses (e.g. Role)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfile());
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddTransient<IDataService, DataService>();
builder.Services.AddTransient<IPDFService, PDFService>();

builder.Services.AddTransient<IBaseService<ArtistViewModel>, ArtistService>();
builder.Services.AddTransient<IBaseService<SongViewModel>, SongService>();
builder.Services.AddTransient<IBaseService<SongSpecificationViewModel>, SongSpecificationService>();
//Cached
builder.Services.AddScoped<ICached<ArtistViewModel>, ArtistCached>();
builder.Services.AddScoped<ICached<SongViewModel>, SongCached>();
builder.Services.AddScoped<ICached<SongSpecificationViewModel>, SongSpecificationCached>();

// In-Memory Caching
builder.Services.AddMemoryCache();

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

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

using var scope = app.Services.CreateScope();
using (scope)
{
    var seeder = scope.ServiceProvider.GetService<DataSeeder>();
    seeder.Seed();
    var service = scope.ServiceProvider.GetService<ServiceCaller>();
    if (service != null) service.ReadData();
}

app.Run();


