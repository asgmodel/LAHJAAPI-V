using Stripe.Checkout;
namespace StripeGateway
{
    public class StripeCheckout(string successPage = "success", string cancelPage = "cancel") : IStripeCheckout
    {

        public string SuccessPage { get; set; } = successPage;
        public string CancelPage { get; set; } = cancelPage;


        public async Task<Session> CreateCheckoutSession(SessionCreateOptions options, CancellationToken cancellationToken = default)
        {
            var service = new SessionService();
            var session = await service.CreateAsync(options, cancellationToken: cancellationToken);
            //var result = mapper.Map<Session>(session);
            return session;
        }

        public async Task<Session> UpdateCheckoutSession(string SessionId, CancellationToken cancellationToken = default)
        {
            var options = new SessionUpdateOptions
            {
                //Metadata = new Dictionary<string, string> { { "order_id", "6735" } },

            };
            var service = new SessionService();
            var session = await service.UpdateAsync(SessionId, options, cancellationToken: cancellationToken);
            //var result = mapper.Map<Session>(session);
            return session;
        }


    }
}