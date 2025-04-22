namespace Dto.Stripe.CustomerDto
{
    public class CustomerResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Object { get; set; }

        //public Address Address { get; set; }

        public long Balance { get; set; }

        //public StripeList<Subscription> Subscriptions { get; set; }

        public bool? Deleted { get; set; }


    }
}
