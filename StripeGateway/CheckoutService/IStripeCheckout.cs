using Stripe.Checkout;

namespace StripeGateway
{
    public interface IStripeCheckout
    {
        string SuccessPage { get; set; }
        string CancelPage { get; set; }

        Task<Session> CreateCheckoutSession(SessionCreateOptions options, CancellationToken cancellationToken = default);

        Task<Session> UpdateCheckoutSession(string SessionId, CancellationToken cancellationToken = default);
    }
}