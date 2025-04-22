using Stripe;

namespace StripeGateway;

public interface IStripeInvoice
{
    Task<Invoice> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<StripeList<Invoice>> GetAllAsync(InvoiceListOptions InvoiceList, CancellationToken cancellationToken = default);

    Task<Invoice> CreateAsync(InvoiceCreateOptions InvoiceCreate, CancellationToken cancellationToken = default);

    Task<Invoice> UpdateAsync(string id, InvoiceUpdateOptions InvoiceUpdate, CancellationToken cancellationToken = default);
    Task<StripeSearchResult<Invoice>> SearchAsync(InvoiceSearchOptions options, CancellationToken cancellationToken = default);
}
