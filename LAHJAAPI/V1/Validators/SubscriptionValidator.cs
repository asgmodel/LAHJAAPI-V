using AutoGenerator.Conditions;
using LAHJAAPI.Models;
using LAHJAAPI.V1.Enums;
using LAHJAAPI.V1.Validators.Conditions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using V1.DyModels.Dso.ResponseFilters;

namespace LAHJAAPI.V1.Validators;

public enum SubscriptionValidatorStates
{
    IsSubscribe = 10000,
    IsActive = 10001,
    IsAllowedRequests = 10002,
    IsNotSubscribe = 6000,
    IsCancelAtPeriodEnd = 6001,
    IsCanceled = 6002,
    IsNotAllowedRequests = 6003,
    IsSubscriptionId,
    IsCustomerId,
    HasStatus,
    IsValidStartDate,
    IsValidPeriodDates,
    IsAllowedSpaces,
    IsFull,
    IsValid,
    IsActiveAndResult,
}


public class SubscriptionValidator : BaseValidator<SubscriptionResponseFilterDso, SubscriptionValidatorStates>, ITValidator
{
    private readonly IConditionChecker _checker;
    private Subscription Subscription { get; set; }
    public SubscriptionValidator(IConditionChecker checker) : base(checker)
    {
        _checker = checker;
        //_dataContext = checker.Injector.DataContext;
        //checker.Injector.DataContext = this;
    }


    protected override void InitializeConditions()
    {
        _provider.Register(
            SubscriptionValidatorStates.IsActive,
            new LambdaCondition<SubscriptionResponseFilterDso>(
                nameof(SubscriptionValidatorStates.IsActive),
                context => IsActive(context),
                "Subscription is not active"
            )
        );

        _provider.Register(
            SubscriptionValidatorStates.IsCanceled,
            new LambdaCondition<SubscriptionResponseFilterDso>(
                nameof(SubscriptionValidatorStates.IsCanceled),
                context => context.Status!.Equals(SubscriptionValidatorStates.IsCanceled.ToString(), StringComparison.OrdinalIgnoreCase),
                "Your subscription has canceled"
            )
        );

        _provider.Register(
            SubscriptionValidatorStates.IsCancelAtPeriodEnd,
            new LambdaCondition<SubscriptionResponseFilterDso>(
                nameof(SubscriptionValidatorStates.IsActive),
                context => context.CancelAtPeriodEnd,
                "Subscription is canceled. Do you want to renew it?"
            )
        );


        _provider.Register(
            SubscriptionValidatorStates.IsSubscribe,
            new LambdaCondition<SubscriptionResponseFilterDso>(
                nameof(SubscriptionValidatorStates.IsSubscribe),
                context => IsSubscribeAsync(context),
                "You are not subscription"
            )
        );

        _provider.Register(
            SubscriptionValidatorStates.IsNotSubscribe,
            new LambdaCondition<SubscriptionResponseFilterDso>(
                nameof(SubscriptionValidatorStates.IsNotSubscribe),
                context => IsNotSubscribeAsync(context),
                "You are not subscription"
            )
        );

        _provider.Register(
            SubscriptionValidatorStates.IsAllowedRequests,
            new LambdaCondition<SubscriptionResponseFilterDso>(
                nameof(SubscriptionValidatorStates.IsAllowedRequests),
                context => IsAllowedRequests(context),
                "You have exhausted all allowed subscription requests."
            )
        );

        _provider.Register(
              SubscriptionValidatorStates.IsSubscriptionId,
              new LambdaCondition<string>(
                  nameof(SubscriptionValidatorStates.IsSubscriptionId),
                  context => isSubscriptionId(context),
                  "Customer Id is required"
              )
          );


        _provider.Register(
            SubscriptionValidatorStates.IsCustomerId,
            new LambdaCondition<string>(
                nameof(SubscriptionValidatorStates.IsCustomerId),
                context => isCustomerId(context),
                "Customer Id is required"
            )
        );

        _provider.Register(
            SubscriptionValidatorStates.IsAllowedSpaces,
            new LambdaCondition<int>(nameof(SubscriptionValidatorStates.IsAllowedSpaces),
            context => IsAllowedSpaces(context),
            "Space not allowed"
        ));

        _provider.Register(
            SubscriptionValidatorStates.IsAllowedSpaces,
            new LambdaCondition<SubscriptionResponseFilterDso>(nameof(SubscriptionValidatorStates.IsAllowedSpaces),
            context => IsAllowedSpaces(context),
            "Space not allowed"
        ));


        _provider.Register(
        SubscriptionValidatorStates.IsActiveAndResult,
        new LambdaCondition<string>(
            nameof(SubscriptionValidatorStates.IsActiveAndResult),
            context => VaildIsServiceIdResultAsync(context),
            "You are not subscription"
        )
    );

    }

    Subscription GetSubscription(SubscriptionResponseFilterDso context)
    {
        Subscription ??= _checker.Injector.Context.Set<Subscription>()
            .Where(s => s.UserId == _checker.Injector.UserClaims.UserId)
            .FirstOrDefault() ?? throw new ArgumentNullException("No subscription found");
        return Subscription;
    }

    bool IsAllowedSpaces(int count)
    {
        var result = _checker.Injector.Context.Set<Subscription>()
            .Where(x => x.UserId == _checker.Injector.UserClaims.UserId)
             .Select(s => new
             {
                 s.AllowedSpaces,
                 SpaceCount = s.Spaces.Count()
             }).FirstOrDefault();


        return result.AllowedSpaces >= result.SpaceCount;
    }


    bool IsAllowedSpaces(params object[] args)
    {
        if (args.Length != 2 || args[0] is not int count || args[1] is not string subId)
            return false;

        var result = _checker.Injector.Context.Set<Subscription>()
            .FirstOrDefault(x => x.Id == subId);

        return result?.AllowedSpaces >= count;
    }

    private bool isSubscriptionId(string subId)
    {
        var result = _checker.Injector.Context.Set<Subscription>()
         .Any(user => user.Id == subId);
        return result;
    }

    bool isCustomerId(string userId)
    {
        return _checker.Check(ApplicationUserValidatorStates.HasCustomerId, userId);

    }

    bool IsActive(SubscriptionResponseFilterDso context)
    {
        GetSubscription(context);
        return Subscription.Status!.Equals("active", StringComparison.OrdinalIgnoreCase);
    }

    bool IsCanceled(SubscriptionResponseFilterDso context)
    {
        GetSubscription(context);
        return Subscription.Status!.Equals("canceled", StringComparison.OrdinalIgnoreCase);
    }

    bool IsCancelAtPeriodEnd(SubscriptionResponseFilterDso context)
    {
        GetSubscription(context);
        return Subscription.CancelAtPeriodEnd;
    }

    bool IsAllowedRequests(SubscriptionResponseFilterDso context)
    {
        var subscription = GetSubscription(context);
        var requests = _checker.Injector.Context.Requests
            .Where(r => r.SubscriptionId == subscription.Id
            && r.Status == RequestStatus.Success.ToString()
            && r.CreatedAt >= subscription.CurrentPeriodStart && r.CreatedAt <= subscription.CurrentPeriodEnd)
            .ToList();

        return subscription.AllowedRequests >= requests.Count;
    }

    bool IsNotAllowedRequests(SubscriptionResponseFilterDso context)
    {
        //await GetSubscription(context);
        return !IsAllowedRequests(context);
    }

    bool IsSubscribeAsync(SubscriptionResponseFilterDso context)
    {
        if (IsCancelAtPeriodEnd(context) || IsCanceled(context)) return false;

        return IsActive(context);
    }

    ProblemDetails? IsNotSubscribeAsync(SubscriptionResponseFilterDso context)
    {
        if (IsCanceled(context))
        {
            return new ProblemDetails
            {
                Title = "Subscription has canceled",
                Detail = "Your subscription has canceled",
                Status = (int)SubscriptionValidatorStates.IsCanceled,
                //Type = "https://example.com/canceled"
            };
        }
        else if (IsCancelAtPeriodEnd(context))
        {
            return new ProblemDetails
            {
                Title = "Subscription will cancel cancel at periodend",
                Detail = "Subscription is canceled. Do you want to renew it?",
                Status = (int)SubscriptionValidatorStates.IsCancelAtPeriodEnd
            };
        }
        else if (!IsActive(context))
        {
            return new ProblemDetails
            {
                Title = "Subscription is not active",
                Detail = "You are not subscription",
                Status = (int)SubscriptionValidatorStates.IsNotSubscribe
            };
        }
        return null;
    }


    private async Task<ConditionResult> VaildIsServiceIdResultAsync(string subId)
    {
        var subscription = GetSubscription(null);
        var result = await _checker.Injector.Context.Subscriptions
                             .Include(s => s.PlanId)

                            .FirstOrDefaultAsync(s => s.Id == subscription.Id);

        if (result.Status!.Equals("active", StringComparison.OrdinalIgnoreCase))
            return new ConditionResult(true, result, "");
        return new ConditionResult(false, null, "error");
    }


}