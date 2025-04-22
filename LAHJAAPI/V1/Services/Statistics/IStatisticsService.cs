using LAHJAAPI.V1.Enums;
using V1.DyModels.VM;

namespace LAHJAAPI.V1.Services.Statistics
{
    public interface IStatisticsService
    {
        Task<IEnumerable<ModelAiServiceData>> GetModelAiServicesRequests();
        Task<IEnumerable<RequestData>> GetRequests(FilterBy filterBy, RequestType requestTypes, DateTime? StartDate, DateTime? EndDate);
        Task<IEnumerable<RequestData>> GetRequestsByDatetime(FilterBy filterBy, RequestType requestTypes, DateTime? StartDate, DateTime? EndDate, DateTimeFilter groupBy);
        Task<IEnumerable<ServiceDataTod>> GetRequestsByStatus(FilterBy filterBy, RequestType requestTypes, DateTime? StartDate, DateTime? EndDate);
        Task<IEnumerable<UsedRequestsVm>> GetServiceUsageAndRemaining();
        Task<List<UsedRequestsVm>> GetServiceUsageDataAsync();
        Task<UsedRequestsVm> GetUsageAndRemainingRequests();
    }
}
