using Microsoft.Extensions.Options;
using Stripe;

namespace StripeGateway;

public class StripeProduct : IStripeProduct
{
    private readonly IStripeClient client;

    public StripeProduct(IOptions<StripeOptions> options)
    {
        this.client = new StripeClient(options.Value.SecretKey);
    }

    public async Task<Product> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var service = new ProductService();
        var product = await service.GetAsync(id, cancellationToken: cancellationToken);
        return product;
    }

    public async Task<StripeList<Product>> GetAllAsync(ProductListOptions options, CancellationToken cancellationToken = default)
    {
        var service = new ProductService();
        StripeList<Product> products = await service.ListAsync(options, cancellationToken: cancellationToken);
        return products;
    }

    public async Task<Product> CreateAsync(ProductCreateOptions options, CancellationToken cancellationToken = default)
    {
        var service = new ProductService();
        var product = await service.CreateAsync(options, cancellationToken: cancellationToken);
        return product;
    }

    public async Task<Product> UpdateAsync(string id, ProductUpdateOptions options, CancellationToken cancellationToken = default)
    {

        var service = new ProductService();
        var product = await service.UpdateAsync(id, options, cancellationToken: cancellationToken);
        //var result = _mapper.Map<ProductResponse>(product);
        return product;
    }


    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var service = new ProductService();
        var response = await service.DeleteAsync(id, cancellationToken: cancellationToken);
        return response.Deleted == true;
    }

    public async Task<StripeSearchResult<Product>> SearchAsync(ProductSearchOptions options = null, CancellationToken cancellationToken = default)
    {
        //var options = new ProductSearchOptions
        //{
        //    Query = query,
        //    Limit = limit,
        //    Page = page
        //    //Query = "active:'true' AND metadata['order_id']:'6735'",
        //};
        //if(page!=null)
        var service = new ProductService();
        StripeSearchResult<Product> products = await service.SearchAsync(options, cancellationToken: cancellationToken);
        var nextpage = products.NextPage;
        //var result = _mapper.Map<StripeList<ProductResponse>>(products);
        return products;
    }
}