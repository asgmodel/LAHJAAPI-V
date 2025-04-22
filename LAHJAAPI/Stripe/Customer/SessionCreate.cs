using System.ComponentModel.DataAnnotations;

namespace Dto.Stripe.Customer
{
    public class SessionCreate
    {
        [Required] public string ReturnUrl { get; set; }
    }
}
