//using Api.Repositories;
//using ASG.ApiService.Repositories;
//using AutoMapper;
//using Dto.Subscription;
//using Microsoft.EntityFrameworkCore;
//using LAHJAAPI.Utilities;
//using VM;

//namespace Api.Services
//{
//    public class SubscriptionService(IMapper mapper,
//                               ISubscriptionRepository subscriptionRepository,
//                               IRequestRepository requestRepository,
//                               ISpaceRepository spaceRepository,
//                               IPlanRepository planRepository,
//                               IUserClaims userClaims
//                                   )
//    {
//        private SubscriptionDto? Subscription { get; set; }

//        private int NumberRequests { get; set; }
//        public async Task<SubscriptionDto> GetSubscription(string? userId = null)
//        {
//            if (Subscription != null) return Subscription;
//            userId ??= userClaims.UserId;
//            var sub = await subscriptionRepository.GetByAsync(s => s.UserId == userId)
//                ?? throw new KeyNotFoundException("You have no subscription");
//            Subscription = mapper.Map<SubscriptionDto>(sub);
//            return Subscription;
//        }
//        public async Task<int> GetNumberRequests()
//        {
//            if (NumberRequests != 0) return NumberRequests;
//            Subscription ??= await GetSubscription();
//            NumberRequests = await requestRepository!.GetCount(
//                Subscription.Id, null,
//                Subscription.CurrentPeriodStart,
//                Subscription.CurrentPeriodEnd,
//                RequestStatus.Completed.GetDisplayName());
//            return NumberRequests;
//        }

//        public async Task<SubscriptionAllowed> GetSubscriptionWithAllowed()
//        {
//            Subscription ??= await GetSubscription();
//            if (Subscription == null) return (Subscription!, NumberRequests, false);
//            NumberRequests = await GetNumberRequests();
//            var Allowed = await IsAllowed();
//            return (Subscription!, NumberRequests!, Allowed);
//        }

//        public async Task<bool> IsAllowed()
//        {
//            NumberRequests = await GetNumberRequests();
//            return NumberRequests <= Subscription!.AllowedRequests;
//        }

//        public async Task<bool> IsFree(string? userId = null)
//        {
//            userId ??= userClaims.UserId;
//            await GetSubscription(userId);
//            var plan = await planRepository.GetByIdAsync(Subscription!.PlanId, "en");
//            return plan.Amount == 0;
//        }

//        public async Task<bool> IsSpaceFound(string spaceId)
//        {
//            Subscription = await GetSubscription();
//            return await spaceRepository.Exists(s => s.SubscriptionId == Subscription.Id && s.Id == spaceId);
//        }

//        public async Task<List<SpaceVM>> GetSpaces(string subscriptionId)
//        {
//            var spaces = await spaceRepository.GetAllAsync(s => s.SubscriptionId == subscriptionId);
//            var result = mapper.Map<List<SpaceVM>>(spaces);
//            return result;
//        }

//        public async Task<SpaceVM> GetSpace(string spaceId)
//        {
//            Subscription = await GetSubscription();
//            var space = await spaceRepository.GetByAsync(s => s.SubscriptionId == Subscription.Id && s.Id == spaceId);
//            var result = mapper.Map<SpaceVM>(space);
//            return result;
//        }

//        public async Task<bool> IsSpaceAvailable(string? userId = null)
//        {
//            userId ??= userClaims.UserId;
//            var sub = await subscriptionRepository.Get()
//                .Where(s => s.UserId == userId).Select(s => new
//                {
//                    s.Id,
//                    s.AllowedSpaces,
//                    SpaceCount = s.Spaces.Count()
//                }).FirstOrDefaultAsync();

//            Subscription = new SubscriptionDto()
//            {
//                Id = sub.Id,
//                AllowedSpaces = sub.AllowedSpaces,
//            };

//            return (sub.SpaceCount <= Subscription.AllowedSpaces);
//        }
//    }

//    public record struct SubscriptionAllowed(SubscriptionDto Subscription, int NumberRequests, bool Allowed)
//    {
//        public static implicit operator (SubscriptionDto? Subscription, int NumberRequests, bool Allowed)(SubscriptionAllowed value)
//        {
//            return (value.Subscription, value.NumberRequests, value.Allowed);
//        }

//        public static implicit operator SubscriptionAllowed((SubscriptionDto Subscription, int NumberRequests, bool Allowed) value)
//        {
//            return new SubscriptionAllowed(value.Subscription, value.NumberRequests, value.Allowed);
//        }
//    }
//}
