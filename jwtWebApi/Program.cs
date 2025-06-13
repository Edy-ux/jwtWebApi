using jwtWebApi.Configuration;
using jwtWebApi.Extentions;
using jwtWebApi.Services.Token;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddDataAnnotationsLocalization();


//Configuration Options 
builder.Services.Configure<ConfigurationOptions>(
    builder.Configuration.GetSection(ConfigurationOptions.JWT));

//Add Services 
builder.Services.AddScoped<ITokenService, JWTTokenService>();
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



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


