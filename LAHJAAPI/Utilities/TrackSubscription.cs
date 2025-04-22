//using Api.Repositories;
//using Api.Results;
//using Api.LAHJAAPI.Utilities;
//using APILAHJA.LAHJAAPI.Utilities;
//using LAHJAAPI.Utilities;

//namespace ASG.ApiService.LAHJAAPI.Utilities
//{
//    public class TrackSubscription
//    {
//        IServiceScopeFactory _serviceScopeFactory;

//        private Lazy<Task<HandelErrors>> _lazyData;
//        // must call refreshData in login 
//        public TrackSubscription(IServiceScopeFactory serviceScopeFactory)
//        {
//            _serviceScopeFactory = serviceScopeFactory;
//            _lazyData = new Lazy<Task<HandelErrors>>(LoadDataAsync);
//        }
//        public void RefreshData()
//        {
//            _lazyData = new Lazy<Task<HandelErrors>>(LoadDataAsync);
//            Reset();
//        }

//        public async Task<HandelErrors> LoadDataAsync()
//        {
//            using var scope = _serviceScopeFactory.CreateScope();
//            // Resolve DataContext from the new scope
//            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
//            var subscriptionRepository = scope.ServiceProvider.GetRequiredService<ISubscriptionRepository>();
//            var planRepository = scope.ServiceProvider.GetRequiredService<IPlanRepository>();
//            var requestRepository = scope.ServiceProvider.GetRequiredService<IRequestRepository>();

//            var user = await userRepository.GetWithSubscription(u => u.Id == UserId);
//            if (user == null) return HandelErrors.Failure(Error.RecordNotFound("No user found"));
//            this.CustomerId = user.CustomerId;

//            var subscription = user.Subscription;
//            if (subscription == null) return HandelErrors.Failure(Error.RecordNotFound("You don't have subscription"));
//            this.SubscriptionId = subscription.Id;
//            this.Status = subscription.Status;
//            this.CancelAtPeriodEnd = subscription.CancelAtPeriodEnd;

//            var plan = await planRepository.GetByIdAsync(subscription.PlanId, "en");

//            var planFeature = plan.PlanFeatures.FirstOrDefault(pf => pf.Name == "Requests");

//            var desc = planFeature.Description;
//            if (desc.Contains("request")) desc = desc[..6];
//            this.NumberRequests = Convert.ToInt32(Convert.ToDecimal(desc));

//            if (!this.IsSubscribe)
//                return HandelErrors.Failure(Error.RecordNotFound("Your subscription is canceled or expired"));

//            this.CurrentNumberRequests = await requestRepository.GetCount(subscription.Id, null, subscription.CurrentPeriodStart, subscription.CurrentPeriodEnd, RequestStatus.Completed.GetDisplayName());

//            if (!this.IsAllowed) return HandelErrors.Failure(new Error("Requests Ended", "You have exhausted all allowed subscription requests.", StatusCodes.Status402PaymentRequired));

//            return HandelErrors.Ok();
//        }


//        public Task<HandelErrors> GetSubscriptionsAsync() => _lazyData.Value;

//        public void Reset()
//        {
//            CustomerId = SubscriptionId = Status = string.Empty;
//            CancelAtPeriodEnd = false;
//            NumberRequests = CurrentNumberRequests = 0;
//        }

//        public string UserId { get; set; }
//        public string? CustomerId { get; set; }
//        public string SubscriptionId { get; set; }
//        public int? NumberRequests { get; set; }
//        public int CurrentNumberRequests { get; set; }
//        public string Status { get; internal set; }
//        public bool CancelAtPeriodEnd { get; internal set; }

//        public bool IsSubscribe => (Status == SubscriptionStatus.Active);
//        public bool IsNotSubscribe => (Status != SubscriptionStatus.Active);
//        public bool Canceled => (Status == SubscriptionStatus.Canceled);
//        public bool IsAllowed => (CurrentNumberRequests <= NumberRequests && NumberRequests != 0);
//        public bool LimitReached => (CurrentNumberRequests > NumberRequests && NumberRequests != 0);
//    }
//}
