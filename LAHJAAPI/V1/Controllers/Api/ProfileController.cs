using APILAHJA.LAHJAAPI.Utilities;
using AutoGenerator;
using AutoGenerator.Helper;
using LAHJAAPI.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using V1.DyModels.Dso.Requests;
using V1.DyModels.VMs;
using V1.Services.Services;

namespace V1.Controllers.Api
{

    [Route("api/V1/Api/[controller]")]
    [ApiController]
    public class ProfileController(
        IUseApplicationUserService userService,
        IUseSubscriptionService subscriptionService,
        IUseUserModelAiService userModelAiService,
        IUseUserServiceService userServiceService,
        IUseSpaceService spaceService,
        IUseRequestService requestService,
        IMapper mapper,
        ILogger<ProfileController> logger,
        IUserClaimsHelper userClaims
        ) : Controller
    {
        [HttpGet("user", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApplicationUserOutputVM>> GetUser()
        {
            var user = await userService.GetOneByAsync([new() { PropertyName = "Id", Value = userClaims.UserId }],
                new ParamOptions() { Includes = ["Subscription"] });
            var response = mapper.Map<ApplicationUserOutputVM>(user);
            return Ok(response);
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Update([FromBody] ApplicationUserUpdateVM model)
        {
            try
            {
                logger.LogInformation("Updating User");
                var user = mapper.Map<ApplicationUserRequestDso>(model);

                await userService.UpdateAsync(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
        }


        [HttpGet("subscriptions", Name = "GetUserSubscriptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedResponse<IEnumerable<SubscriptionOutputVM>>>> GetUserSubscriptions()
        {
            try
            {
                var user = await userService.GetByIdAsync(userClaims.UserId);

                if (user.CustomerId == null)
                    return NotFound(HandelErrors.NotFound("You are not stripe customer yet!", "user", "No customer id found"));

                var response = await subscriptionService.GetAllByAsync([
                    new () { PropertyName="CustomerId",Value=user.CustomerId},
                    //     new (){
                    //    Logic=FilterLogic.And,
                    //    PropertyName = "Status", Value="Active"
                    //}
                    ], new ParamOptions() { PageSize = 100 });

                var response2 = response.ToResponse(mapper.Map<IEnumerable<SubscriptionOutputVM>>(response.Data));
                return Ok(response2);
            }
            catch (Exception ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
        }

        [HttpGet("modelAis", Name = "ModelAis")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<UserModelAiOutputVM>>> ModelAis()
        {
            var response = await userModelAiService.GetAllByAsync([
                new () { PropertyName="UserId",Value=userClaims.UserId}
                ], new ParamOptions() { Includes = ["ModelAi"] });
            var data = mapper.Map<List<UserModelAiOutputVM>>(response.Data);

            return Ok(data);
        }

        [HttpGet("services", Name = "Services")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<ServiceOutputVM>>> Services()
        {
            try
            {
                var response = await userService.GetOneByAsync([
               new FilterCondition{
                    PropertyName="Id",
                    Value=userClaims.UserId
                }], new ParamOptions(["UserServices.Service"]));
                if (response == null) return NoContent();

                var services = response.UserServices?.Select(s => new ServiceOutputVM
                {
                    Id = s.Service?.Id,
                    Name = s.Service?.Name,
                    AbsolutePath = s.Service?.AbsolutePath,
                    ModelAiId = s.Service?.ModelAiId,
                    Token = s.Service?.Token,
                });

                //var result = mapper.Map<List<ServiceOutputVM>>(services);

                return Ok(services);
            }
            catch (Exception ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
        }
        //TODO: Cause looping
        //[HttpGet("services-modelAi/{modelAiId}", Name = "ServicesModelAi")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //public async Task<ActionResult<List<ServiceResponse>>> ServicesModelAi(string modelAiId)
        //{
        //    var services = await serviceRepository.GetAllAsync(s => s.ModelAiId == modelAiId);
        //    var response = mapper.Map<List<ServiceResponse>>(services);

        //    return Ok(response);
        //}


        [HttpGet("spaces-subscription", Name = "SpacesSubscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<SpaceOutputVM>>> SpacesSubscription(string? subscriptionId)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                var items = await spaceService.GetSpacesBySubscriptionId((await subscriptionService.GetUserSubscription()).Id);
                var res = mapper.Map<IEnumerable<SpaceOutputVM>>(items);
                return Ok(res);
            }

            var user = await userService.GetByIdAsync(userClaims.UserId);

            var subscription = await subscriptionService.GetOneByAsync([
                new FilterCondition { PropertyName = "CustomerId",Value = user.CustomerId},
                new FilterCondition { PropertyName = "Id",Value = subscriptionId},
            ]);

            if (subscription == null)
                return BadRequest(HandelErrors.NotFound("Incorrect subscription id or not belong to you.", "Subscription", "No subscription found"));

            var spaces = await spaceService.GetSpacesBySubscriptionId(subscriptionId);
            var response = mapper.Map<IEnumerable<SpaceOutputVM>>(spaces);
            return Ok(response);
        }

        [HttpGet("space-subscription", Name = "SpaceSubscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SpaceOutputVM>> SpaceSubscription(string subscriptionId, string spaceId)
        {
            //var subscription = await subscriptionService.GetUserSubscription();
            //if (subscription == null) return BadRequest(HandelErrors.NotFound("Subscription id incorrect or not belong to you.", "spaces subscription"));

            var item = await subscriptionService.GetSpace(spaceId, subscriptionId);
            var response = mapper.Map<SpaceOutputVM>(item);

            return Ok(response);
        }

        [HttpGet("requests-subscription", Name = "RequestsSubscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedResponse<RequestOutputVM>>> RequestsSubscription(string subscriptionId)
        {
            var response = await requestService.GetAllByAsync([new FilterCondition("SubscriptionId", subscriptionId)], new ParamOptions(["Events"]));
            var result = response.ToResponse(mapper.Map<IEnumerable<RequestOutputVM>>(response.Data));

            return Ok(result);
        }

        [HttpGet("requests-services", Name = "RequestsService")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedResponse<RequestOutputVM>>> RequestsService(string serviceId)
        {
            var response = await requestService.GetAllByAsync([new FilterCondition("ServiceId", serviceId)], new ParamOptions(["Events"]));
            var result = response.ToResponse(mapper.Map<IEnumerable<RequestOutputVM>>(response.Data));

            return Ok(result);
        }
    }
}