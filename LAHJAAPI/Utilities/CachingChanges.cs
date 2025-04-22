using LAHJAAPI.Models;

namespace Api.LAHJAAPI.Utilities
{
    public class ClaimsChange
    {
        public bool IsChange { get; set; }
        public bool SendEmail { get; set; }
        public ApplicationUser? User { get; set; }
        public string? SubscriptionId { get; set; }

    }
}
