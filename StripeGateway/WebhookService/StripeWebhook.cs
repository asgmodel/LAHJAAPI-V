using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Stripe;

namespace StripeGateway
{
    public interface IStripeWebhook
    {
        Event ConstructEvent(string json);
    }

    public class StripeWebhook(IHttpContextAccessor httpContext, IOptions<StripeOptions> options) : IStripeWebhook
    {
        public Event ConstructEvent(string json)
        {
            var stripeEvent = EventUtility.ConstructEvent(json,
                httpContext.HttpContext.Request.Headers["Stripe-Signature"],
                options.Value.WebhookSecret);
            return stripeEvent;
        }

        public void CreateEndPoint()
        {
            var options = new WebhookEndpointCreateOptions
            {
                Url = "https://example.com/my/webhook/endpoint",
                EnabledEvents = new List<String>   {
                    "payment_intent.payment_failed",
                    "payment_intent.succeeded",
                },
            };
            var service = new WebhookEndpointService();
            var endpoint = service.Create(options);
        }
    }
}
