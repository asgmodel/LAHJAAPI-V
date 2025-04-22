using AutoGenerator.Repositories.Base;
using AutoGenerator.Services.Base;
using V1.DyModels.Dso.Requests;
using V1.DyModels.Dso.Responses;

namespace V1.Services.Services
{
    public interface IUsePlanFeatureService : IPlanFeatureService<PlanFeatureRequestDso, PlanFeatureResponseDso>, IBaseService//يمكنك  التزويد بكل  دوال   طبقة Builder   ببوابات  الطبقة   هذه نفسها
    //, IPlanFeatureBuilderRepository<PlanFeatureRequestDso, PlanFeatureResponseDso>
    , IBasePublicRepository<PlanFeatureRequestDso, PlanFeatureResponseDso>
    {
        Task<PlanFeatureResponseDso?> GetByNameAsync(string planId, string name, string lg = "en");
        Task<int> GetNumberRequests(string planId);
        Task<int> GetNumberSpaces(string planId);
    }
}