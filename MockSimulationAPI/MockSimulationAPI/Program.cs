using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/", (PaymentRequest req) =>
{
    var response = new PaymentSimulationResponse
    {
        Status = "Success",
        TransactionId = Guid.NewGuid().ToString(),
        Timestamp = DateTime.UtcNow,
        Message = "Payment processed successfully by mock provider.",
        ReferenceId = req.ReferenceId
    };

    return Results.Ok(response);
});

app.Run();

// Models
public class PaymentRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Description { get; set; }
    public string ReferenceId { get; set; }
}

public class PaymentSimulationResponse
{
    public string Status { get; set; }
    public string TransactionId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Message { get; set; }
    public string ReferenceId { get; set; }
}
