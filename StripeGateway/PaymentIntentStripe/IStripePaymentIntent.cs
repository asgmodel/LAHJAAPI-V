using Stripe;

namespace StripeGateway
{
    public interface IStripePaymentIntent
    {
        Task<PaymentIntent> CreatePaymentIntent(PaymentIntentCreateOptions options, CancellationToken cancellationToken = default);

    }
}