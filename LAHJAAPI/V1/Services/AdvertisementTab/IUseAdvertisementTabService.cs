using AutoGenerator.Repositories.Base;
using AutoGenerator.Services.Base;
using V1.DyModels.Dso.Requests;
using V1.DyModels.Dso.Responses;

namespace V1.Services.Services
{
    public interface IUseAdvertisementTabService : IAdvertisementTabService<AdvertisementTabRequestDso, AdvertisementTabResponseDso>, IBaseService//يمكنك  التزويد بكل  دوال   طبقة Builder   ببوابات  الطبقة   هذه نفسها
    //, IAdvertisementTabBuilderRepository<AdvertisementTabRequestDso, AdvertisementTabResponseDso>
    , IBasePublicRepository<AdvertisementTabRequestDso, AdvertisementTabResponseDso>
    {
    }
}