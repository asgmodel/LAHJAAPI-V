using Stripe;

namespace StripeGateway;

public interface IStripePrice
{
    Task<Price> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<StripeList<Price>> GetAllAsync(PriceListOptions priceList, CancellationToken cancellationToken = default);

    Task<Price> CreateAsync(PriceCreateOptions priceCreate, CancellationToken cancellationToken = default);

    Task<Price> UpdateAsync(string id, PriceUpdateOptions priceUpdate, CancellationToken cancellationToken = default);
    Task<Price> ArchiveAsync(string id, bool isArchive, CancellationToken cancellationToken = default);
    Task<StripeSearchResult<Price>> SearchAsync(PriceSearchOptions options, CancellationToken cancellationToken = default);
}
