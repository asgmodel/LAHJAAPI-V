using AutoMapper;
using Stripe;
namespace StripeGateway
{
    public interface IStripeSetupIntent
    {
        Task<StripeSetuptIntentResponse> CancelAsync(string id);
        Task<StripeSetuptIntentResponse> ConfirmAsync(string id, SetupIntentConfirmOptions options);
        Task<StripeSetuptIntentResponse> CreateAsync(SetupIntentCreateOptions options, CancellationToken cancellationToken = default);
        Task<StripeSetuptIntentResponse> GetAsync(string id);
        Task<List<StripeSetuptIntentResponse>> GetListAsync(SetupIntentListOptions options = null);
        Task<StripeSetuptIntentResponse> UpdateAsync(string id, SetupIntentUpdateOptions options, CancellationToken cancellationToken = default);
    }

    public class StripeSetupIntent(IMapper mapper) : IStripeSetupIntent
    {
        public async Task<StripeSetuptIntentResponse> CreateAsync(SetupIntentCreateOptions options, CancellationToken cancellationToken = default)
        {
            //var options = new SetupIntentCreateOptions
            //{
            //      Customer = "cus_123456789", // معرف العميل في Stripe
            //      PaymentMethodTypes = new List<string> { "card" },
            //};
            var service = new SetupIntentService();
            var setupIntent = await service.CreateAsync(options, cancellationToken: cancellationToken);
            var item = mapper.Map<StripeSetuptIntentResponse>(setupIntent);
            return item;
        }

        public async Task<StripeSetuptIntentResponse> UpdateAsync(string id, SetupIntentUpdateOptions options, CancellationToken cancellationToken = default)
        {
            //var options = new SetupIntentUpdateOptions
            //{
            //    Metadata = new Dictionary<string, string> { { "order_id", "6735" } },
            //};
            var service = new SetupIntentService();
            var setupIntent = await service.UpdateAsync(id, options, cancellationToken: cancellationToken);
            var item = mapper.Map<StripeSetuptIntentResponse>(setupIntent);
            return item;
        }


        public async Task<StripeSetuptIntentResponse> GetAsync(string id)
        {
            var service = new SetupIntentService();
            var setupIntent = await service.GetAsync(id);
            var item = mapper.Map<StripeSetuptIntentResponse>(setupIntent);
            return item;
        }

        public async Task<List<StripeSetuptIntentResponse>> GetListAsync(SetupIntentListOptions options = null)
        {
            //var options = new SetupIntentListOptions { Limit = 3 };
            var service = new SetupIntentService();
            StripeList<SetupIntent> setupIntents = await service.ListAsync(options);
            var items = mapper.Map<List<StripeSetuptIntentResponse>>(setupIntents.Data);
            return items;
        }

        public async Task<StripeSetuptIntentResponse> CancelAsync(string id)
        {
            //var options = new SetupIntentListOptions { Limit = 3 };
            var service = new SetupIntentService();
            var setupIntent = await service.CancelAsync(id);
            var item = mapper.Map<StripeSetuptIntentResponse>(setupIntent);
            return item;
        }

        public async Task<StripeSetuptIntentResponse> ConfirmAsync(string id, SetupIntentConfirmOptions options)
        {
            //var options = new SetupIntentConfirmOptions { PaymentMethod = "pm_card_visa" };
            var service = new SetupIntentService();
            var setupIntent = await service.ConfirmAsync(id, options);
            var item = mapper.Map<StripeSetuptIntentResponse>(setupIntent);
            return item;
        }

    }
}