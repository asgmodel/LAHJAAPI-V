using Newtonsoft.Json;
using Stripe;

namespace StripeGateway
{
    public class PaymentMethodBillingDetailsResponse
    {
        [JsonProperty("address")]
        public Address Address { get; set; }

        /// <summary>
        /// Email address.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Full name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Billing phone number (including extension).
        /// </summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}
