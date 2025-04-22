using Stripe;

namespace StripeGateway;

public class StripeInvoice : IStripeInvoice
{
    private readonly InvoiceService _invoiceService;
    public StripeInvoice()
    {
        _invoiceService = new InvoiceService();
    }

    public async Task<Invoice> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var Invoice = await _invoiceService.GetAsync(id, cancellationToken: cancellationToken);
        return Invoice;
    }

    public async Task<StripeList<Invoice>> GetAllAsync(InvoiceListOptions options, CancellationToken cancellationToken = default)
    {
        StripeList<Invoice> Invoices = await _invoiceService.ListAsync(options, cancellationToken: cancellationToken);
        return Invoices;
    }

    public async Task<Invoice> CreateAsync(InvoiceCreateOptions options, CancellationToken cancellationToken = default)
    {
        Invoice Invoice = await _invoiceService.CreateAsync(options, cancellationToken: cancellationToken);
        return Invoice;
    }

    public async Task<Invoice> UpdateAsync(string id, InvoiceUpdateOptions options, CancellationToken cancellationToken = default)
    {
        var Invoice = await _invoiceService.UpdateAsync(id, options, cancellationToken: cancellationToken);
        //var result = _mapper.Map<InvoiceDto>(Invoice);
        return Invoice;
    }

    public async Task<StripeSearchResult<Invoice>> SearchAsync(InvoiceSearchOptions options, CancellationToken cancellationToken = default)
    {
        StripeSearchResult<Invoice> Invoices = await _invoiceService.SearchAsync(options, cancellationToken: cancellationToken);
        return Invoices;
    }

}
