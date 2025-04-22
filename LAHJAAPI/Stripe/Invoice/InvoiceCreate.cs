using System.ComponentModel.DataAnnotations;

namespace StripeGateway
{
    public class InvoiceCreate
    {
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string SubscriptionId { get; set; }
        public bool AutoAdvance { get; set; }

    }
}
