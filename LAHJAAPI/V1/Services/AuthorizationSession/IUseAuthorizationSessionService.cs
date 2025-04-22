using AutoGenerator.Repositories.Base;
using AutoGenerator.Services.Base;
using FluentResults;
using V1.DyModels.Dso.Requests;
using V1.DyModels.Dso.Responses;

namespace V1.Services.Services
{
    public interface IUseAuthorizationSessionService : IAuthorizationSessionService<AuthorizationSessionRequestDso, AuthorizationSessionResponseDso>, IBaseService//يمكنك  التزويد بكل  دوال   طبقة Builder   ببوابات  الطبقة   هذه نفسها
    //, IAuthorizationSessionBuilderRepository<AuthorizationSessionRequestDso, AuthorizationSessionResponseDso>
    , IBasePublicRepository<AuthorizationSessionRequestDso, AuthorizationSessionResponseDso>
    {
        Task<Result<AuthorizationSessionResponseDso>> GetSessionByServices(string userId, List<string> servicesIds, string authorizationType);
    }
}