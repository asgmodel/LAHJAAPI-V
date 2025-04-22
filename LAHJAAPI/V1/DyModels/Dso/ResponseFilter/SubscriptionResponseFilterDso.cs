using AutoGenerator;
using V1.DyModels.Dto.Share.ResponseFilters;

namespace V1.DyModels.Dso.ResponseFilters
{
    public class SubscriptionResponseFilterDso : SubscriptionResponseFilterShareDto, ITDso
    {
        public int CountRequests { get; set; }
    }
}