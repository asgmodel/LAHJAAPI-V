using AutoMapper;
using LAHJAAPI.Data;
using LAHJAAPI.Models;
using V1.DyModels.Dto.Build.Requests;
using V1.DyModels.Dto.Build.Responses;
using V1.Repositories.Base;

namespace V1.Repositories.Builder
{
    /// <summary>
    /// AuthorizationSession class property for BuilderRepository.
    /// </summary>
     //
    public class AuthorizationSessionBuilderRepository : BaseBuilderRepository<AuthorizationSession, AuthorizationSessionRequestBuildDto, AuthorizationSessionResponseBuildDto>, IAuthorizationSessionBuilderRepository<AuthorizationSessionRequestBuildDto, AuthorizationSessionResponseBuildDto>
    {
        /// <summary>
        /// Constructor for AuthorizationSessionBuilderRepository.
        /// </summary>
        public AuthorizationSessionBuilderRepository(DataContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger) // Initialize  constructor.
        {
            // Initialize necessary fields or call base constructor.
            ///
            /// 

            /// 
        }

        //public async Task<Result<AuthorizationSessionResponseBuildDto>> GetSessionByServices2(string userId,string[] servicesIds, string authorizationType)
        //{
        //    var session = await  GetQueryable()
        //        .Where(s => s.UserId == userId && s.AuthorizationType.ToLower() == authorizationType.ToLower())
        //        .Select(s => new ()
        //        {
        //            Id = s.Id,
        //            Services = JsonConvert.DeserializeObject<string[]>(s.ServicesIds),
        //            EndTime = s.EndTime,
        //            IsActive = s.IsActive,
        //            SessionToken = s.SessionToken,
        //        })
        //        .AsEnumerable()
        //    .FirstOrDefault(s => s.Services.Count() == servicesIds.Count() &&
        //        s.Services.SequenceEqual(servicesIds));
        //    ;



        //    //if (session == null) return Result.Fail(new Error("You have not session to use this service"));
        //    //if (session.EndTime < DateTime.UtcNow) return Result.Fail(new Error("Your session has expired"));
        //    //if (!session.IsActive) return Result.Fail(new Error("Your current session has been suspended."));

        //    return Result.Ok(session);
        //}
    }
}