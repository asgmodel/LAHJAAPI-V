using System.ComponentModel.DataAnnotations;

namespace Dto.Stripe.CheckoutDto
{
    public class CheckoutOptions
    {
        [Required]
        public string PlanId { get; set; }

        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }

    }
}
