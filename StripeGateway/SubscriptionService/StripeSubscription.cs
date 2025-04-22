using AutoMapper;
using Stripe;

namespace StripeGateway
{
    public interface IStripeSubscription
    {
        Task<StripeSubscriptionResponse> CreateAsync(SubscriptionCreateOptions options, CancellationToken cancellationToken = default);
        Task<StripeSubscriptionResponse> CancelAsync(string id, CancellationToken cancellationToken = default);
        Task<StripeList<StripeSubscriptionResponse>> GetAllAsync(string customerId = null, CancellationToken cancellationToken = default);
        Task<StripeSubscriptionResponse> UpdateAsync(string id, SubscriptionUpdateOptions options, CancellationToken cancellationToken = default);
        Task<StripeSubscriptionResponse> ResumeAsync(string id, SubscriptionResumeOptions options, CancellationToken cancellationToken = default);
        Task<StripeSubscriptionResponse> GetByIdAsync(string subscriptionId, SubscriptionGetOptions options = null, CancellationToken cancellationToken = default);
    }

    public class StripeSubscription(IMapper mapper) : IStripeSubscription
    {
        public async Task<StripeSubscriptionResponse> CreateAsync(SubscriptionCreateOptions options, CancellationToken cancellationToken = default)
        {
            var service = new SubscriptionService();
            var subscription = await service.CreateAsync(options, cancellationToken: cancellationToken);
            var item = mapper.Map<StripeSubscriptionResponse>(subscription);
            return item;
        }


        public async Task<StripeSubscriptionResponse> UpdateAsync(string id, SubscriptionUpdateOptions options, CancellationToken cancellationToken = default)
        {
            var service = new SubscriptionService();
            var subscription = await service.UpdateAsync(id, options, cancellationToken: cancellationToken);
            var item = mapper.Map<StripeSubscriptionResponse>(subscription);
            return item;
        }


        public async Task<StripeSubscriptionResponse> ResumeAsync(string id, SubscriptionResumeOptions options, CancellationToken cancellationToken = default)
        {
            var service = new SubscriptionService();
            var subscription = await service.ResumeAsync(id, options, cancellationToken: cancellationToken);
            var item = mapper.Map<StripeSubscriptionResponse>(subscription);
            return item;
        }

        public async Task<StripeList<StripeSubscriptionResponse>> GetAllAsync(string customerId, CancellationToken cancellationToken)
        {
            var service = new SubscriptionService();
            var subscription = await service.ListAsync(new SubscriptionListOptions { Customer = customerId }, cancellationToken: cancellationToken);
            var responses = new StripeList<StripeSubscriptionResponse>
            {
                HasMore = subscription.HasMore,
                Url = subscription.Url,
                Data = []
            };
            responses.Data = mapper.Map<List<StripeSubscriptionResponse>>(subscription.Data);
            return responses;
        }

        public async Task<StripeSubscriptionResponse> GetByIdAsync(string subscriptionId, SubscriptionGetOptions options = null, CancellationToken cancellationToken = default)
        {
            var service = new SubscriptionService();
            var subscription = await service.GetAsync(subscriptionId, options);
            var result = mapper.Map<StripeSubscriptionResponse>(subscription);
            return result;
        }

        public async Task<StripeSubscriptionResponse> CancelAsync(string id, CancellationToken cancellationToken = default)
        {
            var service = new SubscriptionService();
            var subscription = await service.CancelAsync(id, cancellationToken: cancellationToken);
            var result = mapper.Map<StripeSubscriptionResponse>(subscription);
            return result;
        }


    }
}
