using Stripe;
namespace StripeGateway
{
    public class StripePaymentIntent() : IStripePaymentIntent
    {
        public async Task<PaymentIntent> CreatePaymentIntent(PaymentIntentCreateOptions options, CancellationToken cancellationToken = default)
        {
            var service = new PaymentIntentService();
            var item = await service.CreateAsync(options, cancellationToken: cancellationToken);
            return item;
        }
    }
}