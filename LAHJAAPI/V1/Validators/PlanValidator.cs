using AutoGenerator.Conditions;
using LAHJAAPI.Models;
using LAHJAAPI.V1.Validators.Conditions;
using V1.DyModels.Dso.ResponseFilters;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace ApiCore.Validators
{
    public class PlanFeatureValidatorKeys
    {
        public static readonly string NumberModels = "number_models";
        public static readonly string AllowedRequests = "allowed_requests";
        public static readonly string Processor = "processor";
        public static readonly string Ram = "ram";
        public static readonly string Speed = "speed";
        public static readonly string Support = "support";
        public static readonly string Customization = "customization";
    }

    




    public class PlanValidator : BaseValidator<Plan, PlanValidatorStates>, ITValidator
    {
        private readonly ITFactoryInjector _injector;

        private Plan? _plantemp ;

        public PlanValidator(IConditionChecker checker) : base(checker)
        {
            _injector = checker.Injector;
        }


      
        protected override void InitializeConditions()
        {

            RegisterCondition<int>(
                    PlanValidatorStates.HasEnoughModels,
                    PlanFeatureValidatorKeys.NumberModels,
                    CheckHasEnoughModels,
                    "The number of models is less than allowed.");

            RegisterCondition<int>(
                PlanValidatorStates.HasAllowedRequests,
                PlanFeatureValidatorKeys.AllowedRequests,
                CheckHasAllowedRequests,
                "The allowed requests are less than required.");

            RegisterCondition<int>(
                PlanValidatorStates.HasSharedProcessor,
                PlanFeatureValidatorKeys.Processor,
                CheckHasSharedProcessor,
                "Processor is not shared.");

            RegisterCondition<int>(
                PlanValidatorStates.HasEnoughRam,
                PlanFeatureValidatorKeys.Ram,
                CheckHasEnoughRam,
                "RAM is less than required.");

            RegisterCondition<double>(
                PlanValidatorStates.HasSpeedLimit,
                PlanFeatureValidatorKeys.Speed,
                CheckHasSpeedLimit,
                "Speed is less than required.");

            RegisterCondition<string>(
                PlanValidatorStates.SupportDisabled,
                PlanFeatureValidatorKeys.Support,
                CheckSupportDisabled,
                "Support should be 'no' for this plan.");

            RegisterCondition<string>(
                PlanValidatorStates.CustomizationDisabled,
                PlanFeatureValidatorKeys.Customization,
                CheckCustomizationDisabled,
                "Customization should be 'no' for this plan.");
        }

        private void RegisterCondition<T>(
            PlanValidatorStates state,
            string key,
            Func<DataFilter<T, PlanFeature>, Task<ConditionResult>> checkFunc,
            string errorMessage)
        {
            _provider.Register(state,
                new LambdaCondition<DataFilter>(
                    state.ToString(),
                    async context =>
                    {
                        var sharecontext = new DataFilter<T,Plan>(context);
                        var res = await GetPlanFeatureActive(sharecontext, key);
                        return await checkFunc(res);
                    },
                    errorMessage
                ));
        }


        private PlanFeature?  GetFeatureByKey(ICollection<PlanFeature> features,string key) 
        {
            return features.Where(x => x.Key == key).FirstOrDefault();
        }
        private async Task<DataFilter<T, Plan>?> getPlanActive<T>(DataFilter<T,Plan> filter) 
        {

            if (filter.Id != null || filter.Share == null)
                filter.Share= await getPlan(filter.Id);


            return  filter ;





        }


        public async Task<DataFilter<T, PlanFeature>> GetPlanFeatureActive<T>(DataFilter<T, Plan> filter, string key)
        {
            var newfilter=await getPlanActive(filter);
            var  fs=new DataFilter<T, PlanFeature>()
            {
                Id = filter.Id,
                Name = filter.Name,
                Value = filter.Value,

            };

            if (newfilter?.Share == null)
                return fs;

            fs.Share = GetFeatureByKey(newfilter.Share.PlanFeatures, key);
            return fs;

        }

        private Task<ConditionResult> CheckActive(DataFilter<Plan> filter,string key)
        {
            var plan = _injector.Context.Plans.FirstOrDefault(p => p.Id == filter.Id);
            return plan != null && plan.PlanFeatures.Any()
                ? Task.FromResult(new ConditionResult(true, plan, ""))
                : Task.FromResult(new ConditionResult(false, null, "Plan is not active or has no features."));
        }



        private Task<ConditionResult> CheckHasEnoughModels(DataFilter<int,PlanFeature> filter)
        {
            if (filter.Share == null)
            {
                return Task.FromResult(new ConditionResult(false, null, "Feature value is null."));
            }

            if (!int.TryParse(filter.Share.Value, out var value))
            {
                return Task.FromResult(new ConditionResult(false, null, "Invalid value format for models."));
            }

            return value >= filter.Value
                ? Task.FromResult(new ConditionResult(true, filter.Share, ""))
                : Task.FromResult(new ConditionResult(false, null, "The number of models is less than allowed."));
        }

        private Task<ConditionResult> CheckHasAllowedRequests(DataFilter<int,PlanFeature>filter)
        {
            if (filter.Share == null)
            {
                return Task.FromResult(new ConditionResult(false, null, "Feature value is null."));
            }

            if (!int.TryParse(filter.Share.Value, out var value))
            {
                return Task.FromResult(new ConditionResult(false, null, "Invalid value format for allowed requests."));
            }

            return value >= filter.Value
                ? Task.FromResult(new ConditionResult(true, filter.Value, ""))
                : Task.FromResult(new ConditionResult(false, null, "The allowed requests are less than required."));
        }

        private Task<ConditionResult> CheckHasSharedProcessor(DataFilter<int,PlanFeature> filter)
        {
            if (filter.Share == null)
            {
                return Task.FromResult(new ConditionResult(false, null, "Feature value is null."));
            }

            return filter.Share.Value == "shared"
                ? Task.FromResult(new ConditionResult(true, filter.Value, ""))
                : Task.FromResult(new ConditionResult(false, null, "Processor is not shared."));
        }

        private Task<ConditionResult> CheckHasEnoughRam(DataFilter<int,PlanFeature>filter)
        {
            if (filter.Share == null)
            {
                return Task.FromResult(new ConditionResult(false, null, "Feature value is null."));
            }

            if (!int.TryParse(filter.Share.Value, out var value))
            {
                return Task.FromResult(new ConditionResult(false, null, "Invalid value format for RAM."));
            }

            return value >= filter.Value
                ? Task.FromResult(new ConditionResult(true, filter.Value, ""))
                : Task.FromResult(new ConditionResult(false, null, "RAM is less than required."));
        }

        private Task<ConditionResult> CheckHasSpeedLimit(DataFilter<double,PlanFeature>filter)
        {
            if (filter.Share == null)
            {
                return Task.FromResult(new ConditionResult(false, null, "Feature value is null."));
            }

            if (!double.TryParse(filter.Share.Value, out var value))
            {
                return Task.FromResult(new ConditionResult(false, null, "Invalid value format for speed."));
            }

            return value >= filter.Value
                ? Task.FromResult(new ConditionResult(true, filter.Value, ""))
                : Task.FromResult(new ConditionResult(false, null, "Speed is less than required."));
        }

        private Task<ConditionResult> CheckSupportDisabled(DataFilter<string,PlanFeature> filter)
        {
           

            return filter.Share.Value == "no"
                ? Task.FromResult(new ConditionResult(true, filter.Value, ""))
                : Task.FromResult(new ConditionResult(false, null, "Support should be 'no' for this plan."));
        }

        private Task<ConditionResult> CheckCustomizationDisabled(DataFilter<string,PlanFeature> filter)
        {
            if (filter.Share == null)
            {
                return Task.FromResult(new ConditionResult(false, null, "Feature value is null."));
            }

            return filter.Share.Value == "no"
                ? Task.FromResult(new ConditionResult(true, filter.Value, ""))
                : Task.FromResult(new ConditionResult(false, null, "Customization should be 'no' for this plan."));
        }

        private async Task<Plan?> getPlan(string? id)
        {
            if (_plantemp != null && _plantemp.Id == id)
                return _plantemp;
            return await _injector.Context.Plans
                .Where(x => x.Id == id)
                .Include(p => p.PlanFeatures)
                .FirstOrDefaultAsync();
        }

       
    }

    public enum PlanValidatorStates
    {
        IsActive,
        HasEnoughModels,
        HasAllowedRequests,
        HasSharedProcessor,
        HasEnoughRam,
        HasSpeedLimit,
        SupportDisabled,
        CustomizationDisabled
    }
}
