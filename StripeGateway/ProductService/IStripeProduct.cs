using Stripe;

namespace StripeGateway;

public interface IStripeProduct
{
    Task<Product> CreateAsync(ProductCreateOptions priceCreate, CancellationToken cancellationToken = default);

    Task<Product> UpdateAsync(string id, ProductUpdateOptions options, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Product> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<StripeList<Product>> GetAllAsync(ProductListOptions options, CancellationToken cancellationToken = default);
    Task<StripeSearchResult<Product>> SearchAsync(ProductSearchOptions options = null, CancellationToken cancellationToken = default);
}
