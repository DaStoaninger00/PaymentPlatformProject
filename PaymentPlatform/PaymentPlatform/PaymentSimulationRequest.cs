using System.Text.Json.Serialization;

namespace PaymentPlatform
{
    public class PaymentSimulationRequest
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("referenceId")]
        public string ReferenceId { get; set; }
    }
}
