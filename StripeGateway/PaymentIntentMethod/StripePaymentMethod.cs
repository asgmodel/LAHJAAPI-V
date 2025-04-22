using AutoMapper;
using Stripe;
namespace StripeGateway
{
    public interface IStripePaymentMethod
    {
        Task<PaymentMethod> CreateAsync(PaymentMethodCreateOptions options, CancellationToken cancellationToken = default);
        Task<PaymentMethod> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<PaymentMethod> GetAsync(string id);
        Task<PaymentMethod> GetByCustomerAsync(string customerId, string id);
        Task<List<PaymentMethodResponse>> GetListByCustomerAsync(string customerId, CustomerPaymentMethodListOptions options = null);
        Task<PaymentMethod> UpdateAsync(string id, PaymentMethodUpdateOptions options, CancellationToken cancellationToken = default);
    }

    public class StripePaymentMethod(IMapper mapper) : IStripePaymentMethod
    {
        public async Task<PaymentMethod> CreateAsync(PaymentMethodCreateOptions options, CancellationToken cancellationToken = default)
        {
            var service = new PaymentMethodService();
            var item = await service.CreateAsync(options, cancellationToken: cancellationToken);
            return item;
        }

        public async Task<PaymentMethod> UpdateAsync(string id, PaymentMethodUpdateOptions options, CancellationToken cancellationToken = default)
        {
            var service = new PaymentMethodService();
            var item = await service.UpdateAsync(id, options, cancellationToken: cancellationToken);
            return item;
        }

        public async Task<PaymentMethod> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var service = new PaymentMethodService();
            var item = await service.DetachAsync(id, cancellationToken: cancellationToken);
            return item;
        }

        public async Task<PaymentMethod> GetByCustomerAsync(string customerId, string id)
        {
            var service = new CustomerPaymentMethodService();
            var item = await service.GetAsync(customerId, id);
            return item;
        }

        public async Task<List<PaymentMethodResponse>> GetListByCustomerAsync(string customerId, CustomerPaymentMethodListOptions options = null)
        {
            var service = new CustomerPaymentMethodService();
            StripeList<PaymentMethod> methods = await service.ListAsync(customerId, options);
            var items = mapper.Map<List<PaymentMethodResponse>>(methods.Data);
            return items;
        }

        public async Task<PaymentMethod> GetAsync(string id)
        {
            var service = new PaymentMethodService();
            var item = await service.GetAsync(id);
            return item;
        }

        public async Task<StripeList<PaymentMethod>> GetListAsync(PaymentMethodListOptions options = null)
        {
            //var options = new PaymentMethodListOptions
            //{
            //    Type = "card",
            //    Limit = 3,
            //    Customer = "cus_9s6XKzkNRiz8i3",
            //};
            var service = new PaymentMethodService();
            StripeList<PaymentMethod> items = service.List(options);
            return items;
        }
    }
}