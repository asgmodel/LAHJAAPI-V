namespace Dto.Stripe.Payment
{
    public class PaymentResponse
    {
        public required string ClientSecret { get; set; }
    }

    public class CustomerSessionResponse
    {
        public required string CustomerSessionClientSecret { get; set; }
    }
}
