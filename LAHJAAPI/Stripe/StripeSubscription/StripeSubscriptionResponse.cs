namespace Dto.Stripe.StripeSubscription
{
    public class StripeSubscriptionResponse
    {
        /// <summary>
        /// Unique identifier for the object.
        /// </summary>
        public string Id { get; set; }


        #region Expandable Customer

        /// <summary>
        /// (ID of the Customer)
        /// ID of the customer who owns the subscription.
        /// </summary>
        public string CustomerId { get; set; }
        #endregion

    }
}
