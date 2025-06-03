
using jwtWebApi.Configuration;
using jwtWebApi.Services.Token;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers();

builder.Services.AddScoped<ITokenService, JWTTokenService>();

//Configuration Options 
builder.Services.Configure<ConfigurationOptions>(
    builder.Configuration.GetSection(ConfigurationOptions.JWT));

builder.Services.AddAuthentication().AddJwtBearer(options =>
{

    // Configure JWT Bearer authentication
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = false,
        ValidAudience = "myapi",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
           builder.Configuration.GetSection("JWT:Secret_Key").Value!)
            )
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


