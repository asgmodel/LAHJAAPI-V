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

namespace ApiCore.Validators
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

    public class AuthorizationSessionValidator : BaseValidator<AuthorizationSessionResponseFilterDso, SessionValidatorStates>, ITValidator
    {
        DataContext _context;
        private readonly IConditionChecker _checker;

        public AuthorizationSessionValidator(IConditionChecker checker) : base(checker)
        {
            _context = checker.Injector.Context;
            _checker = checker;
        }

        protected override void InitializeConditions()
        {
            _provider.Register(
                SessionValidatorStates.HasSessionToken,
                new LambdaCondition<AuthorizationSessionRequestDso>(
                    nameof(SessionValidatorStates.HasSessionToken),
                    context => Task.FromResult(!string.IsNullOrWhiteSpace(context.SessionToken)
                        ? new ConditionResult(true, context.SessionToken, "")
                        : new ConditionResult(false, null, "Session Token is required"))
                )
            );

            _provider.Register(
                SessionValidatorStates.HasAuthorizationType,
                new LambdaCondition<AuthorizationSessionRequestDso>(
                    nameof(SessionValidatorStates.HasAuthorizationType),
                    context => Task.FromResult(!string.IsNullOrWhiteSpace(context.AuthorizationType)
                        ? new ConditionResult(true, context.AuthorizationType, "")
                        : new ConditionResult(false, null, "Authorization Type is required"))
                )
            );

            _provider.Register(
                SessionValidatorStates.HasStartTime,
                new LambdaCondition<AuthorizationSessionRequestDso>(
                    nameof(SessionValidatorStates.HasStartTime),
                    context => Task.FromResult(context.StartTime != default
                        ? new ConditionResult(true, context.StartTime, "")
                        : new ConditionResult(false, null, "Start Time is required"))
                )
            );

            _provider.Register(
                SessionValidatorStates.IsActive,
                new LambdaCondition<AuthorizationSessionRequestDso>(
                    nameof(SessionValidatorStates.IsActive),
                    context => Task.FromResult(context.IsActive
                        ? new ConditionResult(true, context.IsActive, "")
                        : new ConditionResult(false, null, "Session must be active"))
                )
            );

            _provider.Register(
                SessionValidatorStates.HasUserId,
                new LambdaCondition<AuthorizationSessionRequestDso>(
                    nameof(SessionValidatorStates.HasUserId),
                    context => Task.FromResult(!string.IsNullOrWhiteSpace(context.UserId)
                        ? new ConditionResult(true, context.UserId, "")
                        : new ConditionResult(false, null, "User ID is required"))
                )
            );

            _provider.Register(
                SessionValidatorStates.HasEndTime,
                new LambdaCondition<AuthorizationSessionRequestDso>(
                    nameof(SessionValidatorStates.HasEndTime),
                    context => Task.FromResult(context.EndTime.HasValue
                        ? new ConditionResult(true, context.EndTime.Value, "")
                        : new ConditionResult(false, null, "End Time is required"))
                )
            );

            _provider.Register(
                SessionValidatorStates.IsFound,
                new LambdaCondition<AuthorizationSessionFilterVM>(
                    nameof(SessionValidatorStates.IsFound),
                    context => Task.FromResult(IsFound(context.Id)
                        ? new ConditionResult(true, context.Id, "")
                        : new ConditionResult(false, null, "Session not found"))
                )
            );

            _provider.Register(
                SessionValidatorStates.IsActive,
                new LambdaCondition<AuthorizationSessionFilterVM>(
                    nameof(SessionValidatorStates.IsActive),
                    context => Task.FromResult(IsActive(context.Id)
                        ? new ConditionResult(true, context.Id, "")
                        : new ConditionResult(false, null, "Session not active"))
                )
            );

            _provider.Register(
                SessionValidatorStates.ValidateCoreToken,
                new LambdaCondition<string>(
                    nameof(SessionValidatorStates.ValidateCoreToken),
                    token => Task.FromResult(ValidateCoreToken(token))
                )
            );

            _provider.Register(
                SessionValidatorStates.ValidatePlatformToken,
                new LambdaCondition<string>(
                    nameof(SessionValidatorStates.ValidatePlatformToken),
                    token =>
                    {
                        try
                        {
                            var result = ValidatePlatformToken(token);
                            return Task.FromResult(new ConditionResult(true, result, ""));
                        }
                        catch (Exception ex)
                        {
                            return Task.FromResult(new ConditionResult(false, null, ex.Message));
                        }
                    }
                )
            );

            _provider.Register(
                SessionValidatorStates.IsFull,
                new LambdaCondition<AuthorizationSessionRequestDso>(
                    nameof(SessionValidatorStates.IsFull),
                     context =>
                       {
                           var result = _checker
                                        .GetProvider<SessionValidatorStates>()
                                        .AnyPass(context);
                       
                        return Task.FromResult(new ConditionResult(true, result, ""));



                    }
                )
            );
        }

        AuthorizationSession? Session { get; set; } = null;

        private AuthorizationSession? GetSession(string? id)
        {
            if (Session is not null && Session.Id==id) return Session;
            if (id == null) id = _checker.Injector.UserClaims.SessionId;
            if (string.IsNullOrWhiteSpace(id)) return null;
            return Session = _context.AuthorizationSessions.FirstOrDefault(s => s.Id == id);
        }

        private bool IsFound(string? id) => GetSession(id) is not null;

        private bool IsActive(string? id)
        {
            if (!IsFound(id)) return false;
            var session = GetSession(id);
            return session!.IsActive;
        }

        private DataTokenRequest ValidatePlatformToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new Exception("Token can not be null.");
            string secret = _checker.Injector.AppSettings.Jwt.WebSecret;
            var result = _checker.Injector.TokenService.ValidateToken(token, secret);
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
            var result = _checker.Injector.TokenService.ValidateToken(sessionToken);
            return result.IsSuccess;
        }

        private ConditionResult ValidateCoreToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return new ConditionResult(false, null, "Token can not be null.");

            string secret = _checker.Injector.AppSettings.Jwt.WebSecret;
            var result = _checker.Injector.TokenService.ValidateToken(token, secret);
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
    }
}
