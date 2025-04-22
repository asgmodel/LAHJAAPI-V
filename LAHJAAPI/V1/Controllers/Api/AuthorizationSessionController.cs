using ApiCore.Validators;
using APILAHJA.LAHJAAPI.Utilities;
using ASG.Api2.LAHJAAPI.Utilities;
using AutoGenerator.Helper;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Services2;
using LAHJAAPI.Utilities;
using AutoMapper;
using LAHJAAPI.Utilities;
using LAHJAAPI.V1.Validators.Conditions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using V1.DyModels.Dso.Requests;
using V1.DyModels.Dso.Responses;
using V1.DyModels.VMs;
using V1.Services.Services;

namespace V1.Controllers.Api
{
    [Route("api/V1/Api/[controller]")]
    [ApiController]
    public class AuthorizationSessionController : ControllerBase
    {
        private readonly IUseAuthorizationSessionService _authorizationsessionService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IUseModelGatewayService _modelGatewayRepository;
        private readonly TokenService _tokenService;
        private readonly IUseAuthorizationSessionService _sessionService;
        private readonly IUseServiceService _serviceService;
        private readonly IUseSubscriptionService _subscriptionService;
        private readonly IUseModelAiService _modelAiRepository;
        private readonly IUserClaimsHelper _userClaims;
        private readonly IConditionChecker _checker;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public AuthorizationSessionController(IUseAuthorizationSessionService authorizationsessionService,
            LinkGenerator linkGenerator,
            TokenService tokenService,
            IUseModelGatewayService modelGatewayService,
            IUseAuthorizationSessionService sessionService,
            IUseServiceService serviceService,
            IUseSubscriptionService subscriptionService,
            IUseModelAiService modelAiRepository,
            IUserClaimsHelper userClaims,
            IConditionChecker checker,
            IOptions<AppSettings> appSettings,
            IMapper mapper, ILoggerFactory logger)
        {
            _authorizationsessionService = authorizationsessionService;
            _linkGenerator = linkGenerator;
            _modelGatewayRepository = modelGatewayService;
            _tokenService = tokenService;
            _sessionService = sessionService;
            _serviceService = serviceService;
            _subscriptionService = subscriptionService;
            _modelAiRepository = modelAiRepository;
            _userClaims = userClaims;
            _checker = checker;
            _appSettings = appSettings;
            _mapper = mapper;
            _logger = logger.CreateLogger(typeof(AuthorizationSessionController).FullName);
        }

        // Get all AuthorizationSessions.
        [HttpGet(Name = "GetAuthorizationSessions")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<AuthorizationSessionOutputVM>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all AuthorizationSessions...");
                var result = await _authorizationsessionService.GetAllAsync();
                var items = _mapper.Map<List<AuthorizationSessionOutputVM>>(result);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all AuthorizationSessions");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Get a AuthorizationSession by ID.
        [HttpGet("{id}", Name = "GetAuthorizationSession")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthorizationSessionInfoVM>> GetById(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid AuthorizationSession ID received.");
                return BadRequest("Invalid AuthorizationSession ID.");
            }

            try
            {
                _logger.LogInformation("Fetching AuthorizationSession with ID: {id}", id);
                var entity = await _authorizationsessionService.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("AuthorizationSession not found with ID: {id}", id);
                    return NotFound();
                }

                var item = _mapper.Map<AuthorizationSessionInfoVM>(entity);
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching AuthorizationSession with ID: {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // // Get a AuthorizationSession by Lg.
        [HttpGet("GetAuthorizationSessionByLanguage", Name = "GetAuthorizationSessionByLg")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthorizationSessionOutputVM>> GetAuthorizationSessionByLg(AuthorizationSessionFilterVM model)
        {
            var id = model.Id;
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid AuthorizationSession ID received.");
                return BadRequest("Invalid AuthorizationSession ID.");
            }

            try
            {
                _logger.LogInformation("Fetching AuthorizationSession with ID: {id}", id);
                var entity = await _authorizationsessionService.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("AuthorizationSession not found with ID: {id}", id);
                    return NotFound();
                }

                var item = _mapper.Map<AuthorizationSessionOutputVM>(entity, opt => opt.Items.Add(HelperTranslation.KEYLG, model.Lg));
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching AuthorizationSession with ID: {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // // Get a AuthorizationSessions by Lg.
        [HttpGet("GetAuthorizationSessionsByLanguage", Name = "GetAuthorizationSessionsByLg")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<AuthorizationSessionOutputVM>>> GetAuthorizationSessionsByLg(string? lg)
        {
            if (string.IsNullOrWhiteSpace(lg))
            {
                _logger.LogWarning("Invalid AuthorizationSession lg received.");
                return BadRequest("Invalid AuthorizationSession lg null ");
            }

            try
            {
                var authorizationsessions = await _authorizationsessionService.GetAllAsync();
                if (authorizationsessions == null)
                {
                    _logger.LogWarning("AuthorizationSessions not found  by  ");
                    return NotFound();
                }

                var items = _mapper.Map<IEnumerable<AuthorizationSessionOutputVM>>(authorizationsessions, opt => opt.Items.Add(HelperTranslation.KEYLG, lg));
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching AuthorizationSessions with Lg: {lg}", lg);
                return StatusCode(500, "Internal Server Error");
            }
        }


        [AllowAnonymous]
        [EndpointSummary("Validate Authorization Session")]
        [HttpPost("validate", Name = "AuthorizationSession")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthorizationSessionCoreResponse>> Validate(ValidateTokenRequest validateToken)
        {
            try
            {
                var webToken = _appSettings.Value.Jwt.WebSecret;
                var result = _tokenService.ValidateToken(validateToken.Token, webToken);
                if (result.IsFailed) return Unauthorized(result.Errors); // التوكن غير صالح

                var claims = result.Value;
                var sessionToken = claims.FindFirstValue("SessionToken");

                result = _tokenService.ValidateToken(sessionToken);
                if (result.IsFailed) return BadRequest(result.Errors);
                var item = await _sessionService.GetOneByAsync([new FilterCondition("SessionToken", sessionToken)]);
                if (item == null) return BadRequest("Not found session by token");

                var token = _tokenService.GenerateToken([
               new Claim(ClaimTypes2.ServicesIds, item.ServicesIds),
           new Claim(JwtRegisteredClaimNames.Sub, item.UserId),
                new Claim(ClaimTypes2.SessionId, item.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())]);

                item.UserToken = token;
                await _sessionService.UpdateAsync(_mapper.Map<AuthorizationSessionRequestDso>(item));
                return Ok(new AuthorizationSessionCoreResponse { Token = item.UserToken });
            }
            catch (Exception ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
        }


        // Create a new AuthorizationSession.
        [ServiceFilter(typeof(SubscriptionCheckFilter))]
        [ServiceFilter(typeof(SubscriptionCheckFilter))]
        [HttpPost(Name = "CreateAuthorizationSession")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthorizationSessionOutputVM>> Create([FromBody] AuthorizationSessionCreateVM model)
        {

            try
            {
                _logger.LogInformation("Creating new AuthorizationSession with data: {@model}", model);
                var service = await _serviceService.GetByIdAsync(model.ServiceId);
                bool isNeedSpace = true;
                if (service.AbsolutePath.Equals("createspace", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (await _subscriptionService.AvailableSpace() != 0)
                        return BadRequest(HandelErrors.Problem("Create space", "You cannot create session for a space because you have reached the allowed limit."));
                    isNeedSpace = false;
                    //sessionData.SubscriptionId = trackSubscription.SubscriptionId;
                }
                else if (service.AbsolutePath.Equals("createspace", StringComparison.CurrentCultureIgnoreCase)) isNeedSpace = true;

                //var (type, expire, webToken, spaceId) = await ValidateWebToken(model.Token);
                var response = await PrepareCreateSession([service], model.Token, [model.ServiceId], isNeedSpace);

                var item = _mapper.Map<AuthorizationSessionRequestDso>(model);
                var createdEntity = await _authorizationsessionService.CreateAsync(item);
                var createdItem = _mapper.Map<AuthorizationSessionOutputVM>(createdEntity);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating a new AuthorizationSession");
                //return StatusCode(500, "Internal Server Error");
                return BadRequest(HandelErrors.Problem(ex));
            }
        }


        [EndpointSummary("Create session for dashboard")]
        [HttpPost("CreateForDashboard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthorizationSessionOutputVM>> CreateForDashboard(CreateAuthorizationForDashboard model)
        {
            try
            {
                var service = await _serviceService.GetByAbsolutePath("dashboard");
                if (service == null) return NotFound(HandelErrors.Problem("Create session", "No service found for dahsboard."));
                var response = await PrepareCreateSession([service], model.Token, [service.Id], false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
        }


        [ServiceFilter(typeof(SubscriptionCheckFilter))]
        [EndpointSummary("Create authorization session for list services")]
        [HttpPost("CreateForListServices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthorizationSessionOutputVM>> CreateForListServices(CreateAuthorizationForListServices model)
        {
            try
            {
                //TODO: test this endpoint
                var services = await _serviceService.GetListWithoutSome(model.ServicesIds);
                if (services.Count == 0) return NotFound(HandelErrors.Problem("Create session", "Services ids that you send not aceptable."));
                var servicesIds = services.Select(s => s.Id).ToList();

                var response = await PrepareCreateSession(services, model.Token, model.ServicesIds);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
        }


        [ServiceFilter(typeof(SubscriptionCheckFilter))]
        [EndpointSummary("Create authorization session for all services")]
        [HttpPost("CreateForAllServices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthorizationSessionOutputVM>> CreateForAllServices(CreateAuthorizationForServices model)
        {
            try
            {
                //TODO: test this endpoint
                var services = await _serviceService.GetListWithoutSome(modelId: model.ModelAiId);
                if (services.Count == 0) return NotFound(HandelErrors.Problem("Create session", "Services ids that you send not aceptable.", null, 404));
                var servicesIds = services.Select(s => s.Id).ToList();

                var response = await PrepareCreateSession(services, model.Token, servicesIds);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
        }

        // Update an existing AuthorizationSession.
        [ServiceFilter(typeof(SubscriptionCheckFilter))]
        [HttpPut(Name = "UpdateAuthorizationSession")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthorizationSessionOutputVM>> Update([FromBody] AuthorizationSessionUpdateVM model)
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
                _logger.LogInformation("Updating AuthorizationSession with ID: {id}", model?.Id);
                var item = _mapper.Map<AuthorizationSessionRequestDso>(model);
                var updatedEntity = await _authorizationsessionService.UpdateAsync(item);
                if (updatedEntity == null)
                {
                    _logger.LogWarning("AuthorizationSession not found for update with ID: {id}", model?.Id);
                    return NotFound();
                }

                var updatedItem = _mapper.Map<AuthorizationSessionOutputVM>(updatedEntity);
                return Ok(updatedItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating AuthorizationSession with ID: {id}", model?.Id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Delete a AuthorizationSession.
        [HttpDelete("{id}", Name = "DeleteAuthorizationSession")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid AuthorizationSession ID received in Delete.");
                return BadRequest("Invalid AuthorizationSession ID.");
            }

            try
            {
                _logger.LogInformation("Deleting AuthorizationSession with ID: {id}", id);
                await _authorizationsessionService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting AuthorizationSession with ID: {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Get count of AuthorizationSessions.
        [HttpGet("CountAuthorizationSession")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> Count()
        {
            try
            {
                _logger.LogInformation("Counting AuthorizationSessions...");
                var count = await _authorizationsessionService.CountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while counting AuthorizationSessions");
                return StatusCode(500, "Internal Server Error");
            }
        }



        [HttpPost("encryptFromWeb")]
        public async Task<ActionResult<string>> EncryptFromWebAsync(EncryptTokenRequest encryptToken)
        {
            var webToken = _appSettings.Value.Jwt.WebSecret;
            List<Claim> claims = [new Claim("Data", JsonSerializer.Serialize(encryptToken))];
            //if (encryptToken.Expires != null) claims.Add(new Claim("Expires", encryptToken!.Expires!.ToString()!));
            var encrptedToken = _tokenService.GenerateTemporaryToken(webToken!, claims, encryptToken.Expires);
            return Ok(encrptedToken);
        }

        [HttpGet("encryptFromCore2")]
        public async Task<ActionResult<string>> EncryptFromCoreAsync2(string encrptedToken, string coreToken)
        {
            var decrptedToken = _tokenService.ValidateToken(encrptedToken, coreToken);
            if (decrptedToken.IsFailed) return Unauthorized(decrptedToken.Errors);
            var claims = decrptedToken.ValueOrDefault;
            var sessionToken = claims.FindFirstValue("sessionToken");
            var data = claims.FindFirstValue("data");
            var webToken = claims.FindFirstValue("WebToken");

            var token = _tokenService.GenerateTemporaryToken(webToken, [new Claim("SessionToken", sessionToken)]);

            return Ok(token);
        }

        [HttpGet("ValidateWebTokenAsync")]
        public async Task<ActionResult> ValidateWebTokenAsync(string token)
        {
            var decrptedToken = await ValidateWebToken(token);

            return Ok(new { decrptedToken });
        }

        [HttpGet("ValidateCreateToken")]
        public ActionResult ValidateCreateToken(string token, string coreToken)
        {
            var decrptedToken = _tokenService.ValidateToken(token, coreToken);

            return Ok(new { decrptedToken });
        }

        [HttpGet("ValidateCoreToken")]
        public ActionResult ValidateCoreToken(string token, string coreToken)
        {
            var decrptedToken = _tokenService.ValidateToken(token, coreToken);

            return Ok(new { decrptedToken });
        }

        [EndpointSummary("Pause AuthorizationSession")]
        [HttpPut("pause/{id}", Name = "PauseAuthorizationSession")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Pause(string id)
        {
            try
            {
                var session = await _sessionService.GetByIdAsync(id);
                session.IsActive = false;

                await _sessionService.UpdateAsync(_mapper.Map<AuthorizationSessionRequestDso>(session));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
        }

        //private AuthorizationSessionResponseDso MapToResponse()
        //{

        //}
        [EndpointSummary("Resume AuthorizationSession")]
        [HttpPut("resume/{id}", Name = "ResumeAuthorizationSession")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Resume(string id)
        {
            try
            {
                var session = await _sessionService.GetByIdAsync(id);
                session.IsActive = true;
                await _sessionService.UpdateAsync(_mapper.Map<AuthorizationSessionRequestDso>(session));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
        }

        private async Task<AuthorizationSessionInfoVM> PrepareCreateSession(List<ServiceResponseDso> services, string token, List<string> servicesIds, bool isNeedSpace = true)
        {
            List<Claim> claims = [];
            var sessionData = new SessionData
            {
                Services = services.Select(s => new { s.Id, s.AbsolutePath }).ToList(),
                SubscriptionId = (await _subscriptionService.GetUserSubscription()).Id
            };

            // get platform to validate token

            var dataTokenRequest = (DataTokenRequest?)_checker.CheckAndResult(SessionValidatorStates.ValidateCoreToken, token).Result;
            //var dataTokenRequest = await ValidateWebToken(token);

            if (isNeedSpace)
            {
                if (string.IsNullOrEmpty(dataTokenRequest?.SpaceId))
                    throw new Exception("You must encrypt space id with token.");

                var space = await _subscriptionService.GetSpace(dataTokenRequest.SpaceId);
                if (space == null) throw new Exception("This space is not included in your subscription.");

                sessionData.Space = new { space.Id, space.Name };
                //else throw new Exception($"No space found for id {dataTokenRequest.SpaceId}");
            }

            var modelAi = await _modelAiRepository.GetOneByAsync([new FilterCondition("Id", services[0].ModelAiId)], new ParamOptions(["ModelGateway"]));
            var modelCore = modelAi.ModelGateway;
            if (modelCore == null) throw new Exception("This model ai not belong to model gateway");

            AuthorizationSessionResponseDso session = await GetOrCreateSession(servicesIds, dataTokenRequest.AuthorizationType, dataTokenRequest.Expires, modelAi.Id);
            sessionData.SessionId = session.Id;
            claims.AddRange([new Claim("SessionToken", session.SessionToken),
                    new Claim("ApiUrl", GetApiUrl()!), new Claim("WebToken", dataTokenRequest.Token),new Claim("data",JsonSerializer.Serialize(sessionData))]);

            var encrptedToken = _tokenService.GenerateTemporaryToken(modelCore.Token, claims, session.EndTime);

            string urlCore = $"{modelCore.Url}";
            if (services.Count == 1) urlCore += $"/{services[0].AbsolutePath}";
            //string urlCore = $"{modelCore.Url}/{services.AbsolutePath}";
            return new AuthorizationSessionInfoVM()
            {
                SessionToken = encrptedToken,
                URLCore = urlCore
            };
        }

        private async Task<AuthorizationSessionResponseDso> GetOrCreateSession(List<string> servicesIds, string type, DateTime? expire, string modelAiId)
        {
            var resultSession = await _sessionService.GetSessionByServices(_userClaims.UserId, servicesIds, type);
            var session = resultSession.ValueOrDefault;
            //DateTime endTime = DateTime.UtcNow.AddDays(30);
            if (session == null)
            {
                expire ??= DateTime.UtcNow.AddDays(30);
                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                var newSession = new AuthorizationSessionRequestDso
                {
                    UserId = _userClaims.UserId,
                    EndTime = expire,
                    SessionToken = _tokenService.GenerateToken([new Claim("StartDate", DateTime.UtcNow.ToString())], expires: expire),
                    AuthorizationType = type,
                    IpAddress = ipAddress,
                    DeviceInfo = "",
                    ServicesIds = JsonSerializer.Serialize(servicesIds),
                };
                session = await _sessionService.CreateAsync(newSession);
            }
            else if (resultSession.IsFailed)
            {
                throw new Exception(resultSession.Errors.FirstOrDefault()?.Message);
            }

            return session;
        }

        private string? GetApiUrl()
        {
            return _linkGenerator.GetUriByAction(
                action: nameof(Validate),
                controller: "AuthorizationSession",
                values: null,
                scheme: Request.Scheme,
                host: Request.Host);
        }

        private async Task<DataTokenRequest> ValidateWebToken(string token)
        {
            try
            {
                //var modelGateway = await _modelGatewayRepository.GetWebAsync();
                var webToken = _appSettings.Value.Jwt.WebSecret;
                var result = _tokenService.ValidateToken(token, webToken);
                if (result.IsFailed) throw new Exception(result.Errors.FirstOrDefault().Message);

                var claims = result.Value;
                var dataTokenRequest = JsonSerializer.Deserialize<DataTokenRequest>(claims.FindFirstValue("Data"));
                dataTokenRequest!.Token = webToken;
                return dataTokenRequest;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public class DataTokenRequest
        {
            public string AuthorizationType { get; set; }

            public string? SpaceId { get; set; }
            public DateTime? Expires { get; set; }
            public string Token { get; set; }
        }

        private class SessionData
        {
            public string? SessionId { get; set; }
            public string? SubscriptionId { get; set; }
            public object? Services { get; set; }
            public object? Space { get; set; }
        }
    }
}