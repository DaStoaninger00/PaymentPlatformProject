using System.ComponentModel.DataAnnotations;

namespace PaymentPlatform
{
    public class Provider
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required] public string Name { get; set; }
        [Required, Url] public string Url { get; set; }
        [Required] public string Currency { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }

    }
}
