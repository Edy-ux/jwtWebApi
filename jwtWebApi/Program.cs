using jwtWebApi.Application.Interfaces;
using jwtWebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureModule(builder.Configuration);
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

//Add Localization and Internationalization
app.UseApiInternationalization();

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

// This is a placeholder for the Program class. To be used on test projects or when the main entry point is needed.
public partial class Program { }