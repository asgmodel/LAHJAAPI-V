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


    public  enum TypeGetWay
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

        public ModelGatewayValidator(IConditionChecker checker) : base(checker)
        {
            
        }

        protected override void InitializeConditions()
        {
            //RegisterSimple(ModelGatewayValidatorStates.HasId, ValidateId, "Id is required");
            //RegisterSimple(ModelGatewayValidatorStates.HasName, ValidateName, "Name is required");
            //RegisterSimple(ModelGatewayValidatorStates.HasValidUrl, ValidateUrl, "URL is invalid or missing");
            //RegisterSimple(ModelGatewayValidatorStates.HasTokenIfExists, ValidateToken, "Token cannot be empty if provided");
            //RegisterSimple(ModelGatewayValidatorStates.HasIsDefault, ValidateIsDefault, "");

            RegisterCondition<string>(ModelGatewayValidatorStates.HasModel, ValidateHasModel, "Model is required");
            RegisterCondition<object>(ModelGatewayValidatorStates.IsCore, ValidateIsCore, "Gateway must be of type 'core'");
            RegisterCondition<object>(ModelGatewayValidatorStates.IsWeb, ValidateIsWeb, "Gateway must be of type 'web'");
            RegisterCondition<string>(ModelGatewayValidatorStates.HasUserId, ValidateHasUserId, "UserId is required");
        }

        #region Validation Functions

        private Task<ConditionResult> ValidateId(DataFilter<string, ModelGateway> f)
        {
            bool valid = !string.IsNullOrWhiteSpace(f.Share?.Id);
            return Task.FromResult(new ConditionResult(valid, f.Share, valid ? "" : "Id is required"));
        }

        private Task<ConditionResult> ValidateName(DataFilter<string, ModelGateway> f)
        {
            bool valid = !string.IsNullOrWhiteSpace(f.Share?.Name);
            return Task.FromResult(new ConditionResult(valid, f.Share?.Name, valid ? "" : "Name is required"));
        }

        private Task<ConditionResult> ValidateUrl(DataFilter<string, ModelGateway> f)
        {
            bool valid = Uri.TryCreate(f.Share?.Url, UriKind.Absolute, out _);
            return Task.FromResult(new ConditionResult(valid, f.Share?.Url, valid ? "" : "URL is invalid or missing"));
        }

        private Task<ConditionResult> ValidateToken(DataFilter<string?, ModelGateway> f)
        {
            var token = f.Share?.Token;
            bool valid = token == null || !string.IsNullOrWhiteSpace(token);
            return Task.FromResult(new ConditionResult(valid, token, valid ? "" : "Token cannot be empty if provided"));
        }

        private Task<ConditionResult> ValidateIsDefault(DataFilter<bool, ModelGateway> f)
        {


            return Task.FromResult(new ConditionResult(true, f.Share?.IsDefault ?? false, ""));
        }

        private async Task<ConditionResult> ValidateHasModel(DataFilter<string, ModelGateway> f)
        {

            var getway =await GetModel(f.Id)??null;

            bool isValid = getway !=null;
            return new ConditionResult(isValid, getway, isValid ? "" : "ModelGateway  is  no found");
        }

        private Task<ConditionResult> ValidateIsCore(DataFilter<object, ModelGateway> f)
        {
            bool isValid = string.Equals(f.Share?.Name?.Trim(), TypeGetWay.CORE.ToString(), StringComparison.OrdinalIgnoreCase);
            return Task.FromResult(new ConditionResult(isValid, f.Share, isValid ? "" : "Gateway must be of type 'core'"));
        }

        private Task<ConditionResult> ValidateIsWeb(DataFilter<object, ModelGateway> f)
        {
            bool isValid = string.Equals(f.Share?.Name?.Trim(), TypeGetWay.WEB.ToString(), StringComparison.OrdinalIgnoreCase);
            return Task.FromResult(new ConditionResult(isValid, f.Share, isValid ? "" : "Gateway must be of type 'web'"));
        }

        private Task<ConditionResult> ValidateHasUserId(DataFilter<string, ModelGateway> f)
        {
           return Task.FromResult(new ConditionResult(true, f.Share?.IsDefault ?? false, ""));


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
