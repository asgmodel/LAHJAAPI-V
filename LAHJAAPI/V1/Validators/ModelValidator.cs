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
        
        private Task<ConditionResult> CheckHasCategory(DataFilter<string, ModelAi> f)
        {
            return f.Share?.Category == f.Value
                ? ConditionResult.ToSuccessAsync(f.Share)
                : ConditionResult.ToFailureAsync("Category mismatch.");
        }

        [RegisterConditionValidator(typeof(ModelValidatorStates), ModelValidatorStates.HasLanguage, "Model language does not match the required value.", Value = ModelFeatureValidatorKeys.Language)]
        private Task<ConditionResult> CheckHasLanguage(DataFilter<string, ModelAi> f)
        {
            return f.Share?.Language == f.Value
                ? ConditionResult.ToSuccessAsync(f.Share)
                : ConditionResult.ToFailureAsync("Language mismatch.");
        }

        [RegisterConditionValidator(typeof(ModelValidatorStates), ModelValidatorStates.IsStandardModel, "Model is not marked as standard.", Value = ModelFeatureValidatorKeys.IsStandard)]
        private Task<ConditionResult> CheckIsStandard(DataFilter<bool, ModelAi> f)
        {
            return f.Share?.IsStandard == f.Value
                ? ConditionResult.ToSuccessAsync(f.Share)
                : ConditionResult.ToFailureAsync("Model is not marked as standard.");
        }

        [RegisterConditionValidator(typeof(ModelValidatorStates), ModelValidatorStates.HasDialect, "Model dialect does not match the required value.", Value = ModelFeatureValidatorKeys.Dialect)]
        private Task<ConditionResult> CheckHasDialect(DataFilter<string, ModelAi> f)
        {
            return f.Share?.Dialect == f.Value
                ? ConditionResult.ToSuccessAsync(f.Share)
                : ConditionResult.ToFailureAsync("Dialect mismatch.");
        }

        [RegisterConditionValidator(typeof(ModelValidatorStates), ModelValidatorStates.HasService, "Model service is missing")]
        private async Task<ConditionResult> CheckHasModel(DataFilter<string, ModelAi> f)
        {
            if (f.Share != null)
            {
                var res = await _checker.CheckAndResultAsync(ModelGatewayValidatorStates.HasModel, f.Share.ModelGatewayId);

                if (res.Success==true)
                {
                    f.Share.ModelGateway = (ModelGateway?)res.Result;
                    return ConditionResult.ToSuccess(f.Share);
                }

                return res;
            }

            return ConditionResult.ToFailure(null,"Model AI is not found");
        }

    }
}
