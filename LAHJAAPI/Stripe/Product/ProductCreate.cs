namespace Dto.Stripe.Product
{
    public class ProductCreate
    {
        /// <summary>
        /// The product's name, meant to be displayable to the customer.
        /// </summary>
        public string Name { get; set; }


        public string Description { get; set; }
    }
}
