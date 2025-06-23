using jwtWebApi.Application.Interfaces;
using jwtWebApi.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfraestructureModule(builder.Configuration);
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

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


app.MapGet("/userbytoken/{*tokenId}", async (string tokenId, IUserService service) =>
{

    var user = await service.GetUserByRefreshTokenAsync(tokenId);
    if (user is null)
        return Results.NotFound("User with given token not found");

    return Results.Ok(user);

});


app.Run();

// This is a placeholder for the Program class. To be used on test projects or when the main entry point is needed.
public partial class Program { }