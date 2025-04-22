using System.Collections.Generic;

namespace Dto.Stripe.Product
{
    public class ProductUpdate
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public List<string>? Images { get; set; }
        public bool? Shippable { get; set; }
        public string? UnitLabel { get; set; }
    }
}
