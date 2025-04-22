using ApiCore.Validators;
using APILAHJA.LAHJAAPI.Utilities;
using AutoGenerator.Helper;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Utilities;
using AutoMapper;
using LAHJAAPI.V1.Enums;
using LAHJAAPI.V1.Validators;
using LAHJAAPI.V1.Validators.Conditions;
using Microsoft.AspNetCore.Mvc;
using V1.DyModels.Dso.Requests;
using V1.DyModels.VMs;
using V1.Services.Services;

namespace V1.Controllers.Api
{
    //[ApiExplorerSettings(GroupName = "V1")]
    [Route("api/V1/Api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IUseRequestService _requestService;
        private readonly IUseEventRequestService _eventRequestService;
        private readonly IConditionChecker _checker;
        private readonly IUseSubscriptionService _subscriptionService;
        private readonly IUseServiceService _serviceService;
        private readonly IUserClaimsHelper _userClaims;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public RequestController(
            IUseRequestService requestService,
            IUseEventRequestService eventRequestService,
            IConditionChecker checker,
            IUseSubscriptionService subscriptionService,
            IUseServiceService serviceService,
            IUserClaimsHelper userClaims,
            IMapper mapper, ILoggerFactory logger)
        {
            _requestService = requestService;
            _eventRequestService = eventRequestService;
            _checker = checker;
            _subscriptionService = subscriptionService;
            _serviceService = serviceService;
            _userClaims = userClaims;
            _mapper = mapper;
            _logger = logger.CreateLogger(typeof(RequestController).FullName);
        }

        // Get all Requests.
        [HttpGet(Name = "GetRequests")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RequestOutputVM>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all Requests...");
                var result = await _requestService.GetAllAsync();
                var items = _mapper.Map<List<RequestOutputVM>>(result);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all Requests");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Get a Request by ID.
        [HttpGet("{id}", Name = "GetRequest")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RequestOutputVM>> GetById(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid Request ID received.");
                return BadRequest("Invalid Request ID.");
            }

            try
            {
                _logger.LogInformation("Fetching Request with ID: {id}", id);
                var entity = await _requestService.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Request not found with ID: {id}", id);
                    return NotFound();
                }

                var item = _mapper.Map<RequestOutputVM>(entity);
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching Request with ID: {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // // Get a Request by Lg.
        [HttpGet("GetRequestByLanguage", Name = "GetRequestByLg")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RequestOutputVM>> GetRequestByLg(RequestFilterVM model)
        {
            var id = model.Id;
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid Request ID received.");
                return BadRequest("Invalid Request ID.");
            }

            try
            {
                _logger.LogInformation("Fetching Request with ID: {id}", id);
                var entity = await _requestService.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Request not found with ID: {id}", id);
                    return NotFound();
                }

                var item = _mapper.Map<RequestOutputVM>(entity, opt => opt.Items.Add(HelperTranslation.KEYLG, model.Lg));
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching Request with ID: {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // // Get a Requests by Lg.
        [HttpGet("GetRequestsByLanguage", Name = "GetRequestsByLg")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RequestOutputVM>>> GetRequestsByLg(string? lg)
        {
            if (string.IsNullOrWhiteSpace(lg))
            {
                _logger.LogWarning("Invalid Request lg received.");
                return BadRequest("Invalid Request lg null ");
            }

            try
            {
                var requests = await _requestService.GetAllAsync();
                if (requests == null)
                {
                    _logger.LogWarning("Requests not found  by  ");
                    return NotFound();
                }

                var items = _mapper.Map<IEnumerable<RequestOutputVM>>(requests, opt => opt.Items.Add(HelperTranslation.KEYLG, lg));
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching Requests with Lg: {lg}", lg);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Create a new Request.
        [HttpPost(Name = "CreateRequest")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RequestOutputVM>> Create([FromBody] RequestCreateVM model)
        {
            try
            {
                var subscription = await _subscriptionService.GetUserSubscription();

                if (!_checker.Check(SubscriptionValidatorStates.IsAllowedRequests, new SubscriptionFilterVM() { }))
                    return new ObjectResult(HandelErrors.Problem("Create request", "You have exhausted all allowed subscription requests.", null, (int)SubscriptionValidatorStates.IsNotAllowedRequests))
                    { StatusCode = StatusCodes.Status402PaymentRequired };

                string errorMessage = string.Empty;
                if (!_checker.CheckWithError(ServiceValidatorStates.IsServiceIdFound, model.ServiceId, out errorMessage))
                    return NotFound(HandelErrors.Problem("Service id not in token", errorMessage));


                if (!_checker.Check(SessionValidatorStates.IsFound, new AuthorizationSessionFilterVM(null, null)))
                    return BadRequest(HandelErrors.Problem("Session not found", "This session not found"));
                if (!_checker.Check(SessionValidatorStates.IsActive, new AuthorizationSessionFilterVM(null, null)))
                    return BadRequest(HandelErrors.Problem("Create request", "This session has been suspended."));

                if (!_checker.Check(SpaceValidatorStates.IsFound, new SpaceFilterVM(model.SpaceId, null)))
                    return BadRequest(HandelErrors.Problem("Create request", $"This space is not included in your subscription."));


                var service = await _serviceService.GetOneByAsync(
                    [new FilterCondition("Id", model.ServiceId)],
                    new ParamOptions(["ModelAi.ModelGateway"]));

                if (service == null) return NotFound(HandelErrors.Problem("Create request", "This service not found"));
                var modelAi = service.ModelAi;
                var modelGateway = modelAi.ModelGateway;

                RequestRequestDso request = new()
                {
                    Status = RequestStatus.Processing.ToString(),
                    Question = model.Question,
                    ModelGateway = modelGateway.Url,
                    ModelAi = modelAi.AbsolutePath,
                    UserId = _userClaims.UserId,
                    ServiceId = service.Id,
                    SpaceId = model.SpaceId,
                    SubscriptionId = subscription.Id
                };


                var coreUrl = $" {request.ModelGateway}/{service.AbsolutePath}";
                var eventRequest = new EventRequestRequestDso()
                {
                    Status = RequestStatus.Created.GetDisplayName(),
                    RequestId = request.Id,
                    Details = $"Request has been created for {coreUrl}."
                };
                request.Events.Add(eventRequest);

                _logger.LogInformation("Creating new Request with data: {@model}", request);
                await _requestService.CreateAsync(request);

                return Ok(new RequestInfoVM
                {
                    ModelGateway = request.ModelGateway,
                    ModelAi = request.ModelAi,
                    Service = service.AbsolutePath,
                    Token = service.Token,
                    EventId = eventRequest.Id ?? request.Id,
                    AllowedRequests = subscription.AllowedRequests,
                    NumberRequests = await _subscriptionService.GetNumberRequests(),
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating a new Request");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [ServiceFilter(typeof(SubscriptionCheckFilter))]
        [EndpointSummary("Update Request")]
        [HttpPost("CreateEvent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EventRequestOutputVM>> CreateEvent(EventRequestCreateVM eventRequestCreate)
        {
            //var requestId = "";
            try
            {
                //var subscription = await subscriptionRepository.GetSubscription();

                var eventRequest = await _eventRequestService.GetByIdAsync(eventRequestCreate.EventId);
                if (eventRequest == null) return BadRequest(HandelErrors.Problem("Create Event", "EventId not found."));
                if (eventRequest.Status != RequestStatus.Created.GetDisplayName())
                    return BadRequest(HandelErrors.Problem("Create Event", "This event not acceptable."));



                var requestId = eventRequest.RequestId;
                var request = await _requestService.GetByIdAsync(requestId);

                if (request.Status == RequestStatus.Success.ToString())
                    return BadRequest(HandelErrors.Problem("Create Event", "This request has completed."));
                request.Status = eventRequestCreate.Status.ToString();
                request.UpdatedAt = DateTime.UtcNow;

                var newEventRequest = new EventRequestRequestDso
                {
                    Status = RequestStatus.Success.GetDisplayName(),
                    RequestId = requestId,
                };

                if ((int)eventRequestCreate.Status == (int)RequestStatus.Success)
                {
                    request.Answer = eventRequestCreate.Details;
                    newEventRequest.Details = $"Request has been completed for {request.ModelGateway}.";
                }
                else newEventRequest.Details = eventRequestCreate.Details;

                await _requestService.ExecuteTransactionAsync(async () =>
                {
                    //await _eventRequestService.CreateAsync(newEventRequest);
                    await _eventRequestService.CreateAsync(newEventRequest);
                    var requestVm = _mapper.Map<RequestOutputVM>(request);
                    var requestRequest = _mapper.Map<RequestRequestDso>(requestVm);
                    //requestRequest.Events.Add(newEventRequest);
                    await _requestService.UpdateAsync(requestRequest);
                    return true;

                });
                return Ok(_mapper.Map<EventRequestOutputVM>(newEventRequest));

                //return BadRequest(HandelErrors.Problem("Create Event", "Transaction failed."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating a new Request");
                return BadRequest(HandelErrors.Problem(ex));
            }
        }

        // Update an existing Request.
        [HttpPut(Name = "UpdateRequest")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RequestOutputVM>> Update([FromBody] RequestUpdateVM model)
        {
            if (model == null)
            {
                _logger.LogWarning("Invalid data in Update.");
                return BadRequest("Invalid data.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state in Update: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Updating Request with ID: {id}", model?.Id);
                var item = _mapper.Map<RequestRequestDso>(model);
                var updatedEntity = await _requestService.UpdateAsync(item);
                if (updatedEntity == null)
                {
                    _logger.LogWarning("Request not found for update with ID: {id}", model?.Id);
                    return NotFound();
                }

                var updatedItem = _mapper.Map<RequestOutputVM>(updatedEntity);
                return Ok(updatedItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating Request with ID: {id}", model?.Id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Delete a Request.
        [HttpDelete("{id}", Name = "DeleteRequest")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid Request ID received in Delete.");
                return BadRequest("Invalid Request ID.");
            }

            try
            {
                _logger.LogInformation("Deleting Request with ID: {id}", id);
                await _requestService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting Request with ID: {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Get count of Requests.
        [HttpGet("CountRequest")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> Count()
        {
            try
            {
                _logger.LogInformation("Counting Requests...");
                var count = await _requestService.CountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while counting Requests");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}