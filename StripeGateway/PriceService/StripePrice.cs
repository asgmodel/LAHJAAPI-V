using Microsoft.Extensions.Options;
using Stripe;

namespace StripeGateway;

public class StripePrice : IStripePrice
{
    private readonly IStripeClient client;
    private readonly IOptions<StripeOptions> _options;
    public StripePrice(IOptions<StripeOptions> options)
    {
        _options = options;
        this.client = new StripeClient(_options.Value.SecretKey);
    }

    public async Task<Price> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var service = new PriceService();
        var price = await service.GetAsync(id, cancellationToken: cancellationToken);
        return price;
    }

    public async Task<StripeList<Price>> GetAllAsync(PriceListOptions options, CancellationToken cancellationToken = default)
    {
        //var options = new PriceListOptions
        //{
        //    Active = priceList.Active,
        //    Currency = priceList.Currency,
        //    LookupKeys = priceList.LookupKeys,
        //    Product = priceList.Product,
        //    Type = priceList.Type,
        //};
        var service = new PriceService();
        StripeList<Price> prices = await service.ListAsync(options, cancellationToken: cancellationToken);
        return prices;
    }

    public async Task<Price> CreateAsync(PriceCreateOptions options, CancellationToken cancellationToken = default)
    {
        //var options = new PriceCreateOptions
        //{
        //    Currency = priceCreate.Currency,
        //    //UnitAmountDecimal = priceCreate.UnitAmountDecimal,
        //    UnitAmount = Convert.ToInt64(priceCreate.UnitAmountDecimal) * 100,
        //    Recurring = new PriceRecurringOptions { Interval = priceCreate.Interval },
        //    Product = priceCreate.ProductId,
        //    //LookupKey = "not_plan"
        //    //ProductData = new PriceProductDataOptions { Name = "Gold Plan" }, // this for create new product
        //};
        var service = new PriceService();
        Price price = await service.CreateAsync(options, cancellationToken: cancellationToken);
        return price;
    }

    public async Task<Price> UpdateAsync(string id, PriceUpdateOptions options, CancellationToken cancellationToken = default)
    {
        //var options = new PriceUpdateOptions
        //{
        //    Active = priceUpdate.Active,
        //    //Metadata = new Dictionary<string, string> { { "order_id", "6735" } },
        //};
        //if (priceUpdate.LookupKey != null) options.LookupKey = priceUpdate.LookupKey;

        var service = new PriceService();
        var price = await service.UpdateAsync(id, options, cancellationToken: cancellationToken);
        //var result = _mapper.Map<PriceDto>(price);
        return price;
    }

    public async Task<Price> ArchiveAsync(string id, bool isArchive, CancellationToken cancellationToken = default)
    {
        var service = new PriceService();
        var price = await service.UpdateAsync(id, new PriceUpdateOptions { Active = !isArchive }, cancellationToken: cancellationToken);
        //var result = _mapper.Map<PriceDto>(price);
        return price;
    }

    public async Task<StripeSearchResult<Price>> SearchAsync(PriceSearchOptions options, CancellationToken cancellationToken = default)
    {
        //var options = new PriceSearchOptions
        //{
        //    Query = query,
        //    //Limit = 3,
        //    //Page = "1"
        //    //Query = "active:'true' AND metadata['order_id']:'6735'",
        //};
        var service = new PriceService();
        StripeSearchResult<Price> prices = await service.SearchAsync(options, cancellationToken: cancellationToken);
        return prices;
    }

}
