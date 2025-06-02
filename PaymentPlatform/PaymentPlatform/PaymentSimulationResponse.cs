using System.Text.Json.Serialization;

namespace PaymentPlatform
{
    public class PaymentSimulationResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("transactionId")]
        public string TransactionId { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("referenceId")]
        public string ReferenceId { get; set; }
    }
}
