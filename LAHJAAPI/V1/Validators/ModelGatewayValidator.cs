using ApiCore.Validators;
using ApiCore.Validators.Conditions;
using AutoGenerator.Conditions;
using LAHJAAPI.Models;
using LAHJAAPI.V1.Validators.Conditions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using V1.DyModels.Dso.ResponseFilters;

namespace LAHJAAPI.Validators
{
    public enum TypeGetWay
    {
        API,
        CORE,
        WEB,
    }

    public enum ModelGatewayValidatorStates
    {
        HasId,
        HasName,
        HasValidUrl,
        HasTokenIfExists,
        HasIsDefault,
        HasModel,
        IsCore,
        IsWeb,
        IsApi,
        HasUserId
    }

    public class ModelGatewayValidator : ValidatorContext<ModelGateway, ModelGatewayValidatorStates>
    {
        private ModelGateway? _gateway;

        public ModelGatewayValidator(IConditionChecker checker) : base(checker) { }

        protected override void InitializeConditions()
        {
        }

        #region Validation Functions

        [RegisterConditionValidator(typeof(ModelGatewayValidatorStates), ModelGatewayValidatorStates.HasId, "Id is required")]
        private Task<ConditionResult> ValidateId(DataFilter<string, ModelGateway> f)
        {
            bool valid = !string.IsNullOrWhiteSpace(f.Share?.Id);
            return valid
                ? ConditionResult.ToSuccessAsync(f.Share)
                : ConditionResult.ToFailureAsync("Id is required");
        }

        [RegisterConditionValidator(typeof(ModelGatewayValidatorStates), ModelGatewayValidatorStates.HasName, "Name is required")]
        private Task<ConditionResult> ValidateName(DataFilter<string, ModelGateway> f)
        {
            bool valid = !string.IsNullOrWhiteSpace(f.Share?.Name);
            return valid
                ? ConditionResult.ToSuccessAsync(f.Share)
                : ConditionResult.ToFailureAsync("Name is required");
        }

        [RegisterConditionValidator(typeof(ModelGatewayValidatorStates), ModelGatewayValidatorStates.HasValidUrl, "URL is invalid or missing")]
        private Task<ConditionResult> ValidateUrl(DataFilter<string, ModelGateway> f)
        {
            bool valid = Uri.TryCreate(f.Share?.Url, UriKind.Absolute, out _);
            return valid
                ? ConditionResult.ToSuccessAsync(f.Share)
                : ConditionResult.ToFailureAsync("URL is invalid or missing");
        }

        [RegisterConditionValidator(typeof(ModelGatewayValidatorStates), ModelGatewayValidatorStates.HasTokenIfExists, "Token cannot be empty if provided")]
        private Task<ConditionResult> ValidateToken(DataFilter<string?, ModelGateway> f)
        {
            var token = f.Share?.Token;
            bool valid = token == null || !string.IsNullOrWhiteSpace(token);
            return valid
                ? ConditionResult.ToSuccessAsync(token)
                : ConditionResult.ToFailureAsync("Token cannot be empty if provided");
        }

        [RegisterConditionValidator(typeof(ModelGatewayValidatorStates), ModelGatewayValidatorStates.HasIsDefault, "")]
        private Task<ConditionResult> ValidateIsDefault(DataFilter<bool, ModelGateway> f)
        {
            return ConditionResult.ToSuccessAsync(f.Share?.IsDefault ?? false);
        }

        [RegisterConditionValidator(typeof(ModelGatewayValidatorStates), ModelGatewayValidatorStates.HasModel, "Model is required")]
        private async Task<ConditionResult> ValidateHasModel(DataFilter<string, ModelGateway> f)
        {
            var gateway = await GetModel(f.Id) ?? null;
            return gateway != null
                ? ConditionResult.ToSuccess(gateway)
                : ConditionResult.ToFailure("ModelGateway is not found");
        }

        [RegisterConditionValidator(typeof(ModelGatewayValidatorStates), ModelGatewayValidatorStates.IsCore, "Gateway must be of type 'core'")]
        private Task<ConditionResult> ValidateIsCore(DataFilter<object, ModelGateway> f)
        {
            bool isValid = string.Equals(f.Share?.Name?.Trim(), TypeGetWay.CORE.ToString(), StringComparison.OrdinalIgnoreCase);
            return isValid
                ? ConditionResult.ToSuccessAsync(f.Share)
                : ConditionResult.ToFailureAsync("Gateway must be of type 'core'");
        }

        [RegisterConditionValidator(typeof(ModelGatewayValidatorStates), ModelGatewayValidatorStates.IsWeb, "Gateway must be of type 'web'")]
        private Task<ConditionResult> ValidateIsWeb(DataFilter<object, ModelGateway> f)
        {
            bool isValid = string.Equals(f.Share?.Name?.Trim(), TypeGetWay.WEB.ToString(), StringComparison.OrdinalIgnoreCase);
            return isValid
                ? ConditionResult.ToSuccessAsync(f.Share)
                : ConditionResult.ToFailureAsync("Gateway must be of type 'web'");
        }

        [RegisterConditionValidator(typeof(ModelGatewayValidatorStates), ModelGatewayValidatorStates.HasUserId, "UserId is required")]
        private Task<ConditionResult> ValidateHasUserId(DataFilter<string, ModelGateway> f)
        {
            return ConditionResult.ToSuccessAsync(f.Share?.IsDefault ?? false);
        }

        #endregion

        #region Helpers

        protected override async Task<ModelGateway?> GetModel(string? id)
        {
            if (_gateway != null && _gateway.Id == id)
                return _gateway;

            _gateway = await base.GetModel(id);
            return _gateway;
        }

        #endregion
    }
}
