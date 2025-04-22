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
        public static readonly string Category = "category";
        public static readonly string Language = "language";
        public static readonly string IsStandard = "is_standard";
        public static readonly string Dialect = "dialect";
    }

    public class ModelAIValidator : ValidatorContext<ModelAi, ModelValidatorStates>, ITValidator
    {
        public ModelAIValidator(IConditionChecker checker) : base(checker)
        {
        }

        protected override void InitializeConditions()
        {
            RegisterCondition<string>(
                ModelValidatorStates.HasCategory,
                CheckHasCategory,
                "Model category does not match the required value.",
                ModelFeatureValidatorKeys.Category
                );

            RegisterCondition<string>(
                ModelValidatorStates.HasLanguage,
                CheckHasLanguage,
                "Model language does not match the required value.",
                ModelFeatureValidatorKeys.Language
                );

            RegisterCondition<bool>(
                ModelValidatorStates.IsStandardModel,
                CheckIsStandard,
                "Model is not marked as standard.",
                ModelFeatureValidatorKeys.IsStandard


                );

            RegisterCondition<string>(
                ModelValidatorStates.HasDialect,
                CheckHasDialect,
                "Model dialect does not match the required value.",
                ModelFeatureValidatorKeys.Dialect);


            RegisterCondition<string>(
                ModelValidatorStates.HasService,
                CheckHasModel,
                "Model dialect does not match the required value."
               );



            // 
        }

       

        private Task<ConditionResult> CheckHasCategory(DataFilter<string, ModelAi> filter)
        {
            return filter.Share?.Category == filter.Value
                ? Task.FromResult(new ConditionResult(true, filter.Share, ""))
                : Task.FromResult(new ConditionResult(false, null, "Category mismatch."));
        }

        private Task<ConditionResult> CheckHasLanguage(DataFilter<string, ModelAi> filter)
        {
            return filter.Share?.Language == filter.Value
                ? Task.FromResult(new ConditionResult(true, filter.Share, ""))
                : Task.FromResult(new ConditionResult(false, null, "Language mismatch."));
        }

        private Task<ConditionResult> CheckIsStandard(DataFilter<bool, ModelAi> filter)
        {
            return filter.Share?.IsStandard == filter.Value
                ? Task.FromResult(new ConditionResult(true, filter.Share, ""))
                  : Task.FromResult(new ConditionResult(false, null, "Dialect mismatch."));
        }

        private Task<ConditionResult> CheckHasDialect(DataFilter<string, ModelAi> filter)
        {
            return filter.Share?.Dialect == filter.Value
                ? Task.FromResult(new ConditionResult(true, filter.Share, ""))
                : Task.FromResult(new ConditionResult(false, null, "Dialect mismatch."));
        }

        private  async Task<ConditionResult> CheckHasModel(DataFilter<string, ModelAi> filter)
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
                    return res;
            }
            return   new ConditionResult(false, null, "Model Ai is no found");




        }



  




    }
}