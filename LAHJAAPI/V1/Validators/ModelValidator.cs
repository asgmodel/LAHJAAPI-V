using ApiCore.Validators.Conditions;
using AutoGenerator.Conditions;
using LAHJAAPI.Models;
using LAHJAAPI.V1.Validators.Conditions;
using LAHJAAPI.Validators;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using V1.DyModels.Dso.ResponseFilters;

namespace ApiCore.Validators
{
    public enum ModelValidatorStates
    {
        HasCategory,
        HasLanguage,
        IsStandardModel,
        HasDialect,
        HasService,
    }

    public class ModelFeatureValidatorKeys
    {
        public const string Category = "category";
        public const  string Language = "language";
        public const  string IsStandard = "is_standard";
        public const  string Dialect = "dialect";
    }

    public class ModelAIValidator : ValidatorContext<ModelAi, ModelValidatorStates>, ITValidator
    {
        public ModelAIValidator(IConditionChecker checker) : base(checker)
        {
        }

        protected override void InitializeConditions()
        {
            // All conditions now registered via attributes
        }

        [RegisterConditionValidator(typeof(ModelValidatorStates), ModelValidatorStates.HasCategory, "Model category does not match the required value.", Value = ModelFeatureValidatorKeys.Category)]
        private Task<ConditionResult> CheckHasCategory(DataFilter<string, ModelAi> filter)
        {
            return filter.Share?.Category == filter.Value
                ? Task.FromResult(new ConditionResult(true, filter.Share, ""))
                : Task.FromResult(new ConditionResult(false, null, "Category mismatch."));
        }

        [RegisterConditionValidator(typeof(ModelValidatorStates), ModelValidatorStates.HasLanguage, "Model language does not match the required value.", Value = ModelFeatureValidatorKeys.Language)]
        private Task<ConditionResult> CheckHasLanguage(DataFilter<string, ModelAi> filter)
        {
            return filter.Share?.Language == filter.Value
                ? Task.FromResult(new ConditionResult(true, filter.Share, ""))
                : Task.FromResult(new ConditionResult(false, null, "Language mismatch."));
        }

        [RegisterConditionValidator(typeof(ModelValidatorStates), ModelValidatorStates.IsStandardModel, "Model is not marked as standard.", Value = ModelFeatureValidatorKeys.IsStandard)]
        private Task<ConditionResult> CheckIsStandard(DataFilter<bool, ModelAi> filter)
        {
            return filter.Share?.IsStandard == filter.Value
                ? Task.FromResult(new ConditionResult(true, filter.Share, ""))
                : Task.FromResult(new ConditionResult(false, null, "Model is not marked as standard."));
        }

        [RegisterConditionValidator(typeof(ModelValidatorStates), ModelValidatorStates.HasDialect, "Model dialect does not match the required value.", Value = ModelFeatureValidatorKeys.Dialect)]
        private Task<ConditionResult> CheckHasDialect(DataFilter<string, ModelAi> filter)
        {
            return filter.Share?.Dialect == filter.Value
                ? Task.FromResult(new ConditionResult(true, filter.Share, ""))
                : Task.FromResult(new ConditionResult(false, null, "Dialect mismatch."));
        }

        [RegisterConditionValidator(typeof(ModelValidatorStates), ModelValidatorStates.HasService, "Model service is missing")]
        private async Task<ConditionResult> CheckHasModel(DataFilter<string, ModelAi> filter)
        {
            if (filter.Share != null)
            {
                var res = await _checker.CheckAndResultAsync(ModelGatewayValidatorStates.HasModel, filter.Share.ModelGatewayId);

                if (res.Success == true)
                {
                    filter.Share.ModelGateway = (ModelGateway?)res.Result;
                    return new ConditionResult(true, filter.Share, "");
                }
                else
                {
                    return res;
                }
            }

            return new ConditionResult(false, null, "Model AI is not found");
        }
    }
}
