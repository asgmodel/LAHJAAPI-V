using AutoGenerator.Conditions;
using LAHJAAPI.Models;
using LAHJAAPI.V1.Validators.Conditions;
using V1.DyModels.Dso.Requests;
using V1.DyModels.Dso.ResponseFilters;

namespace LAHJAAPI.V1.Validators
{



    public enum ApplicationUserValidatorStates
    {
        IsActive,
        IsFull,
        HasCustomerId,
        HasFirstName,
        HasLastName,
        HasDisplayName,
        HasProfileUrl,
        HasImageUrl,
        HasLastLoginIp,
        HasCreatedAt,
        HasUpdatedAt,
        IsNotArchived,
        HasArchivedDate,
        HasLastLoginDate


    }


    public class ApplicationUserValidator : BaseValidator<ApplicationUserResponseFilterDso, ApplicationUserValidatorStates>, ITValidator
    {

        private readonly IConditionChecker _checker;
        public ApplicationUserValidator(IConditionChecker checker) : base(checker)
        {

            _checker = checker;
        }
        protected override void InitializeConditions()
        {



            _provider.Register(
                    ApplicationUserValidatorStates.HasLastLoginDate,
                    new LambdaCondition<ApplicationUserRequestDso>(
                        nameof(ApplicationUserValidatorStates.HasLastLoginDate),
                        context => context.LastLoginDate.HasValue,
                        "Last Login Date is required"
                    )
                );

            _provider.Register(
                    ApplicationUserValidatorStates.HasArchivedDate,
                    new LambdaCondition<ApplicationUserRequestDso>(
                        nameof(ApplicationUserValidatorStates.HasArchivedDate),
                        context => context.ArchivedDate.HasValue,
                        "Archived Date is required"
                    )
                );



            _provider.Register(
                    ApplicationUserValidatorStates.HasLastLoginIp,
                    new LambdaCondition<ApplicationUserRequestDso>(
                        nameof(ApplicationUserValidatorStates.HasLastLoginIp),
                        context => !string.IsNullOrWhiteSpace(context.LastLoginIp),
                        "Last Login IP is required"
                    )
                );


            _provider.Register(
                    ApplicationUserValidatorStates.HasImageUrl,
                    new LambdaCondition<ApplicationUserRequestDso>(
                        nameof(ApplicationUserValidatorStates.HasImageUrl),
                        context => !string.IsNullOrWhiteSpace(context.Image),
                        "Image URL is required"
                    )
                );

            _provider.Register(
                   ApplicationUserValidatorStates.IsActive,
                   new LambdaCondition<string>(
                       nameof(ApplicationUserValidatorStates.IsActive),

                       context => isAcive(context),
                       "Space is not active"
                   )
               );

            // First registration for HasCustomerId condition
            _provider.Register(
                ApplicationUserValidatorStates.HasCustomerId,
                new LambdaCondition<string>(
                    nameof(ApplicationUserValidatorStates.HasCustomerId),
                    context => !string.IsNullOrWhiteSpace(context),
                    "Customer ID is required"
                )
            );

            // Next, you have a condition for ApplicationUserRequestDso
            _provider.Register(
                       ApplicationUserValidatorStates.IsFull,
                       new LambdaCondition<ApplicationUserRequestDso>(
                           nameof(ApplicationUserValidatorStates.IsFull),
                           context => IsValidCustomer(context),  //  √ﬂœ √‰ IsValidCustomer  ﬁ»· ‰Ê⁄ ApplicationUserRequestDso
                           "Customer ID is required"
                       )
                   );





        }





        bool isAcive(string id)
        {
            var result = _checker.Injector.Context.Set<ApplicationUser>()
                   .FirstOrDefault(x => x.Id == id);

            return result?.IsArchived ?? false;

        }

        bool isCustomerId(string id)
        {
            var result = _checker.Injector.Context.Set<ApplicationUser>()
           .Any(user => user.Id == id);

            return result;

        }


        private bool IsValidCustomer(ApplicationUserRequestDso context)
        {
            var conditions = new List<Func<ApplicationUserRequestDso, bool>>
    {
        c => !string.IsNullOrWhiteSpace(c.CustomerId),
        c => !string.IsNullOrWhiteSpace(c.FirstName),
        c => !string.IsNullOrWhiteSpace(c.LastName),
        c => !string.IsNullOrWhiteSpace(c.DisplayName),
        c => !string.IsNullOrWhiteSpace(c.ProfileUrl),
        c => !string.IsNullOrWhiteSpace(c.Image),
        c => !string.IsNullOrWhiteSpace(c.LastLoginIp),
        c => c.LastLoginDate.HasValue,
        c => c.CreatedAt != default,
        c => c.UpdatedAt != default,
        c => !c.IsArchived || c.ArchivedDate.HasValue,
        };


            return conditions.All(condition => condition(context));
        }






    }
}