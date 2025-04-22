using LAHJAAPI.V1.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using V1.Services.Services;

public class SubscriptionCheckFilter(IUseSubscriptionService subscriptionService) : IAsyncActionFilter
{

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            var result = await subscriptionService.IsNotSubscribe();
            if (result.IsNotSubscribed)
            {
                context.Result = new ObjectResult(result.Result)
                { StatusCode = StatusCodes.Status402PaymentRequired };
                return;
            }
        }
        catch (Exception ex)
        {
            //if(ex.GetType() == typeof(ArgumentNullException)) 
            context.Result = new ObjectResult(new ProblemDetails
            {
                Title = "Check subscription",
                Detail = "You do not have a subscription. Please subscribe if you want use this service.",
                Status = (int)SubscriptionValidatorStates.IsNotSubscribe
            })
            { StatusCode = StatusCodes.Status402PaymentRequired };
            return;
        }
        await next();
    }


}

