using jwtWebApi.Application.Interfaces;
using jwtWebApi.Configuration;
using jwtWebApi.Context;
using jwtWebApi.Extentions;
using jwtWebApi.Services.Token;
using jwtWebApi.Services.Users;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers()
    .AddDataAnnotationsLocalization();



//Configuration Options 
builder.Services.Configure<ConfigurationOptions>(
    builder.Configuration.GetSection(ConfigurationOptions.JWT));


//builder.Services.AddSingleton<AppDbContextFactory>();

builder.Services.AddDbContextFactory<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//Add Services 

builder.Services.AddScoped<ITokenService, JWTTokenService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddApiInternacionalization();
builder.Services.AddAuthenticationService(builder.Configuration);



var app = builder.Build();

//Add Localization and Internationalization
app.UseApiInternacionalization();

// Configure the HTTP request pipeline.i
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger
    // app.UseSwaggerUI();
}



//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();


public partial class Program { }