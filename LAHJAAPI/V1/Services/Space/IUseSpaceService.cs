using AutoGenerator.Repositories.Base;
using AutoGenerator.Services.Base;
using V1.DyModels.Dso.Requests;
using V1.DyModels.Dso.Responses;

namespace V1.Services.Services
{
    public interface IUseSpaceService : ISpaceService<SpaceRequestDso, SpaceResponseDso>, IBaseService//يمكنك  التزويد بكل  دوال   طبقة Builder   ببوابات  الطبقة   هذه نفسها
    //, ISpaceBuilderRepository<SpaceRequestDso, SpaceResponseDso>
    , IBasePublicRepository<SpaceRequestDso, SpaceResponseDso>
    {
        Task<IEnumerable<SpaceResponseDso>> GetSpacesBySubscriptionId(string subscriptionId);
    }
}