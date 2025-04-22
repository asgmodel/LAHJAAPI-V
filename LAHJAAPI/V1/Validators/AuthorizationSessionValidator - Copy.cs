using ApiCore.Validators;
using ApiCore.Validators.Conditions;
using AutoGenerator.Conditions;
using FluentResults;
using LAHJAAPI.Data;
using LAHJAAPI.Models;
using LAHJAAPI.V1.Validators;
using LAHJAAPI.V1.Validators.Conditions;
using System.Security.Claims;
using System.Text.Json;
using V1.DyModels.Dso.Requests;
using V1.DyModels.Dso.ResponseFilters;
using V1.DyModels.VMs;

namespace ApiCore.Validators.v1
{
    public enum SessionValidatorStates
    {
        HasSessionToken,
        HasAuthorizationType,
        HasStartTime,
        IsActive,
        HasUserId,
        HasEndTime,
        IsFull,
        IsFound,
        ValidateCoreToken,
        ValidatePlatformToken
    }

    public class AuthorizationSessionValidator : ValidatorContext<AuthorizationSession, SessionValidatorStates>, ITValidator
    {
        private AuthorizationSession? Session { get; set; } = null;

        public AuthorizationSessionValidator(IConditionChecker checker) : base(checker)
        {
           
        }

        protected override void InitializeConditions()
        {

        }

        #region Validation Functions

        [RegisterConditionValidator(typeof(SessionValidatorStates), SessionValidatorStates.HasSessionToken, "Session Token is required")]
        private Task<ConditionResult> ValidateHasSessionToken(AuthorizationSessionRequestDso context)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(context.SessionToken)
                ? new ConditionResult(true, context.SessionToken, "")
                : new ConditionResult(false, null, "Session Token is required"));
        }

        [RegisterConditionValidator(typeof(SessionValidatorStates), SessionValidatorStates.HasAuthorizationType, "Authorization Type is required")]
        private Task<ConditionResult> ValidateHasAuthorizationType(AuthorizationSessionRequestDso context)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(context.AuthorizationType)
                ? new ConditionResult(true, context.AuthorizationType, "")
                : new ConditionResult(false, null, "Authorization Type is required"));
        }

        [RegisterConditionValidator(typeof(SessionValidatorStates), SessionValidatorStates.HasStartTime, "Start Time is required")]
        private Task<ConditionResult> ValidateHasStartTime(AuthorizationSessionRequestDso context)
        {
            return Task.FromResult(context.StartTime != default
                ? new ConditionResult(true, context.StartTime, "")
                : new ConditionResult(false, null, "Start Time is required"));
        }

        [RegisterConditionValidator(typeof(SessionValidatorStates), SessionValidatorStates.IsActive, "Session must be active")]
        private Task<ConditionResult> ValidateIsActive(AuthorizationSessionRequestDso context)
        {
            return Task.FromResult(context.IsActive
                ? new ConditionResult(true, context.IsActive, "")
                : new ConditionResult(false, null, "Session must be active"));
        }

        [RegisterConditionValidator(typeof(SessionValidatorStates), SessionValidatorStates.HasUserId, "User ID is required")]
        private Task<ConditionResult> ValidateHasUserId(AuthorizationSessionRequestDso context)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(context.UserId)
                ? new ConditionResult(true, context.UserId, "")
                : new ConditionResult(false, null, "User ID is required"));
        }

        [RegisterConditionValidator(typeof(SessionValidatorStates), SessionValidatorStates.HasEndTime, "End Time is required")]
        private Task<ConditionResult> ValidateHasEndTime(AuthorizationSessionRequestDso context)
        {
            return Task.FromResult(context.EndTime.HasValue
                ? new ConditionResult(true, context.EndTime.Value, "")
                : new ConditionResult(false, null, "End Time is required"));
        }

        [RegisterConditionValidator(typeof(SessionValidatorStates), SessionValidatorStates.IsFound, "Session not found")]
        private Task<ConditionResult> ValidateIsFound(AuthorizationSessionFilterVM context)
        {
            return Task.FromResult(IsFound(context.Id)
                ? new ConditionResult(true, context.Id, "")
                : new ConditionResult(false, null, "Session not found"));
        }

        [RegisterConditionValidator(typeof(SessionValidatorStates), SessionValidatorStates.IsActive, "Session not active")]
        private Task<ConditionResult> ValidateIsActive(AuthorizationSessionFilterVM context)
        {
            return Task.FromResult(IsActive(context.Id)
                ? new ConditionResult(true, context.Id, "")
                : new ConditionResult(false, null, "Session not active"));
        }

        [RegisterConditionValidator(typeof(SessionValidatorStates), SessionValidatorStates.ValidateCoreToken, "Core token validation failed")]
        private Task<ConditionResult> ValidateCoreToken(string token)
        {
            var result = ValidateCoreTokenLogic(token);
            return Task.FromResult(result);
        }

        [RegisterConditionValidator(typeof(SessionValidatorStates), SessionValidatorStates.ValidatePlatformToken, "Platform token validation failed")]
        private Task<ConditionResult> ValidatePlatformToken(string token)
        {
            try
            {
                var result = ValidatePlatformTokenLogic(token);
                return Task.FromResult(new ConditionResult(true, result, ""));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ConditionResult(false, null, ex.Message));
            }
        }

        [RegisterConditionValidator(typeof(SessionValidatorStates), SessionValidatorStates.IsFull, "Session is full")]
        private Task<ConditionResult> ValidateIsFull(AuthorizationSessionRequestDso context)
        {
            var result = _checker.GetProvider<SessionValidatorStates>().AnyPass(context);
            return Task.FromResult(new ConditionResult(true, result, ""));
        }

        #endregion

        #region Helpers

        private AuthorizationSession? GetSession(string? id)
        {
            if (Session is not null && Session.Id == id) return Session;
            if (id == null) id = _injector.UserClaims.SessionId;
            if (string.IsNullOrWhiteSpace(id)) return null;
            return Session = _injector.Context.AuthorizationSessions.FirstOrDefault(s => s.Id == id);
        }



        private bool IsFound(string? id) => GetSession(id) is not null;

        private bool IsActive(string? id)
        {
            if (!IsFound(id)) return false;
            var session = GetSession(id);
            return session!.IsActive;
        }

        private ConditionResult ValidateCoreTokenLogic(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return new ConditionResult(false, null, "Token can not be null.");

            string secret = _injector.AppSettings.Jwt.WebSecret;
            var result = _injector.TokenService.ValidateToken(token, secret);
            if (result.IsFailed)
                return new ConditionResult(false, null, result.Errors?.FirstOrDefault()?.Message ?? "Token validation failed.");

            var claims = result.Value;
            var sessionToken = claims.FindFirstValue("SessionToken");
            if (string.IsNullOrWhiteSpace(sessionToken))
                return new ConditionResult(false, null, "Session token is not found in core token.");

            if (CheckSessionToken(sessionToken))
                return new ConditionResult(true, sessionToken, "");

            return new ConditionResult(false, null, "Session token is invalid.");
        }

        private DataTokenRequest ValidatePlatformTokenLogic(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new Exception("Token can not be null.");
            string secret = _injector.AppSettings.Jwt.WebSecret;
            var result = _injector.TokenService.ValidateToken(token, secret);
            if (result.IsFailed) throw new Exception(result.Errors?.FirstOrDefault()?.Message);
            var claims = result.Value;
            var data = claims.FindFirstValue("Data");
            if (string.IsNullOrWhiteSpace(data)) throw new Exception("Data can not be null.");
            var dataTokenRequest = JsonSerializer.Deserialize<DataTokenRequest>(data);
            dataTokenRequest!.Token = secret;
            return dataTokenRequest;
        }

        private bool CheckSessionToken(string sessionToken)
        {
            if (string.IsNullOrWhiteSpace(sessionToken)) return false;
            var result = _injector.TokenService.ValidateToken(sessionToken);
            return result.IsSuccess;
        }

        #endregion
    }
}
