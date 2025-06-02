using PaymentPlatform;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DataStore>();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.MapGet("/api/providers", (DataStore store) => Results.Ok(store.GetAll()));

app.MapGet("/api/providers/{id}", (Guid id, DataStore store) =>
    store.GetById(id) is Provider provider ? Results.Ok(provider) : Results.NotFound());

app.MapPost("/api/providers", (Provider provider, DataStore store) =>
{
    var existingProvider = store.GetAll();
    if (provider.Id == Guid.Empty || existingProvider.Any(a => a.Id == provider.Id))
        provider.Id = Guid.NewGuid();
    store.Add(provider);
    return Results.Created($"/api/providers/{provider.Id}", provider);
});

app.MapPut("/api/providers/{id}", (Guid id, Provider updatedProvider, DataStore store) =>
    store.Update(updatedProvider, id) ? Results.Ok(updatedProvider) : Results.NotFound());

app.MapDelete("/api/providers/{id}", (Guid id, DataStore store) =>
    store.Remove(id) ? Results.NoContent() : Results.NotFound());

JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

app.MapPost("/api/simulate", async (HttpRequest httpRequest, PaymentSimulationRequest req, DataStore store, HttpClient client) =>
{
    if (!httpRequest.Query.TryGetValue("providerId", out var providerIdValue) || !Guid.TryParse(providerIdValue, out var providerId))
        return Results.BadRequest("Missing or invalid providerId");

    var provider = store.GetById(providerId);
    if (provider is null || !provider.IsActive)
        return Results.BadRequest("Invalid or Inactive Provider");

    //if (!string.Equals(req.Currency, provider.Currency, StringComparison.OrdinalIgnoreCase))
    if(req.Currency != provider.Currency)
        return Results.BadRequest($"Invalid Currency {req.Currency} not supported by provider expected {provider.Currency}");

    var response = await client.PostAsJsonAsync(provider.Url, req);
    if (!response.IsSuccessStatusCode)
        return Results.BadRequest("Payment simulation failed");

    var responseContent = await response.Content.ReadAsStringAsync();
    var result = JsonSerializer.Deserialize<PaymentSimulationResponse>(responseContent, options);
    return Results.Ok(result);

});

app.MapGet("/admin", () =>
{
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../build/dist/index.html");
    if (!File.Exists(filePath)) return Results.NotFound();
    return Results.File(filePath, "text/html");
});

app.MapGet("/assets/{fileName}", (string fileName) =>
{
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../build/dist/assets", fileName);
    if (!File.Exists(filePath)) return Results.NotFound();

    var contentType = "text/html";
    if (fileName.EndsWith(".js")) contentType = "application/javascript";
    else if (fileName.EndsWith(".css")) contentType = "text/css";
    else if (fileName.EndsWith(".map")) contentType = "application/json";

    return Results.File(filePath, contentType);
});

app.Run();
public partial class Program { }
