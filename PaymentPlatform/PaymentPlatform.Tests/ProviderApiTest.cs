using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace PaymentPlatform.Tests
{
    public class ProviderApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProviderApiTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Get_Providers_Should_Return_OK_And_ProviderList()
        {
            var response = await _client.GetAsync("/api/providers");
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var format = await response.Content.ReadFromJsonAsync<List<Provider>>();
            Assert.NotNull(format);
        }

        [Fact]
        public async Task Post_Provider_Should_Create_Provider()
        {
            var provider = new
            {
                name = "Test Provider",
                url = "http://localhost:5005",
                currency = "USD",
                description = "Test_description",
                isActive = true
            };

            var response = await _client.PostAsJsonAsync("/api/providers", provider);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<Provider>();
            Assert.NotNull(created);
            created.Name.Should().Be("Test Provider");
        }

        [Fact]
        public async Task Simulate_Payment_Should_Return_Success_If_Provider_Is_Valid()
        {
            var provider = new
            {
                name = "Test Mock Provider",
                url = "https://localhost:7252",
                currency = "USD",
                description = "",
                isActive = true
            };

            var createResponse = await _client.PostAsJsonAsync("/api/providers", provider);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await createResponse.Content.ReadFromJsonAsync<Provider>();
            Assert.NotNull(created);

            var payload = new
            {
                amount = 110.50m,
                currency = "USD",
                description = "Test Transaction",
                referenceId = "ORDER-12345"
            };

            var url = $"/api/simulate?providerId={created.Id}";
            var response = await _client.PostAsJsonAsync(url, payload);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
