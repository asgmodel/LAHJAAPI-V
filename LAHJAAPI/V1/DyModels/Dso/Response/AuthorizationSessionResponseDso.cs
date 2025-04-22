using AutoGenerator;
using V1.DyModels.Dto.Share.Responses;

namespace V1.DyModels.Dso.Responses
{
    public class AuthorizationSessionResponseDso : AuthorizationSessionResponseShareDto, ITDso
    {
        public List<string>? Services { get; set; }
    }
}