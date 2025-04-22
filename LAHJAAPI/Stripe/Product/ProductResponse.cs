namespace Dto.Stripe.Product
{
    public class ProductResponse
    {
        //
        // Summary:
        //     Unique identifier for the object.
        public string Id { get; set; }

        //
        // Summary:
        //     The product's name, meant to be displayable to the customer.
        public string Name { get; set; }

        //
        // Summary:
        //     String representing the object's type. Objects of the same type share the same
        //     value.
        public string Object { get; set; }
        //
        // Summary:
        //     Whether the product is currently available for purchase.
        public bool Active { get; set; }

        public string Description { get; set; }

        public bool? Deleted { get; set; }
        //
        // Summary:
        //     A list of up to 8 URLs of images for this product, meant to be displayable to
        //     the customer.
        public List<string> Images { get; set; }

        //
        // Summary:
        //     Whether this product is shipped (i.e., physical goods).
        public bool? Shippable { get; set; }

        //
        // Summary:
        //     The type of the product. The product is either of type good, which is eligible
        //     for use with Orders and SKUs, or service, which is eligible for use with Subscriptions
        //     and Plans. One of: good, or service.
        public string Type { get; set; }

        //
        // Summary:
        //     A label that represents units of this product. When set, this will be included
        //     in customers' receipts, invoices, Checkout, and the customer portal.
        public string UnitLabel { get; set; }


        //
        // Summary:
        //     A URL of a publicly-accessible webpage for this product.
        public string Url { get; set; }
    }
}
