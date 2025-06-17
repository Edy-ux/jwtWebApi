using System.Net.Http.Headers;
using System.Text.Json;
using JwtWebApi.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;


namespace JwtWebApi.Tests.Tests.Controllers
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {

            _client = factory.WithWebHostBuilder(builder =>
            {
                // Configurações adicionais do servidor de teste, se necessário
                // builder.UseSetting("https_port", "0"); // evita erro de HTTPS

            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false, // evita redirecionamentos automáticos
                BaseAddress = new Uri("http://localhost")
            });
        }

        [Theory]
        [InlineData("pt-BR", "Sucesso no registo do usuário com login: {0} em:  {1:MMMM dd, yyyy}")]
        [InlineData("en-US", "User registered with login {0} successfully on {1: MMMM dd, yyyy}")]
        public async Task Should_Return_LocalizedValue(string culture, string expectedMessage)
        {
            // Arrange
            _client.DefaultRequestHeaders.AcceptLanguage.Clear();
            _client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));

            // Act
            var response = await _client.GetAsync("api/v1/auth/test-localizer");
            response.EnsureSuccessStatusCode(); // Garante 200 OK

            var json = await response.Content.ReadAsStringAsync();

            var jsonObject = JsonSerializer.Deserialize<JsonElement>(json);

            var message = jsonObject.GetProperty("translatedMessage").GetString();

            // Assert
            Assert.Contains(LocalizationHelper.RemovePlaceholders(expectedMessage), LocalizationHelper.RemovePlaceholders(message!));
        }




    }


}
