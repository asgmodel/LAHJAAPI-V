using Api.LAHJAAPI.Utilities;
using AutoMapper;
using LAHJAAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using StripeGateway;
using LAHJAAPI.Utilities.Services2;
using V1.DyModels.Dso.Requests;
using V1.Services.Services;
using LAHJAAPI.Models;
using Subscription = LAHJAAPI.Models.Subscription;

namespace Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class WebhookController(
    IStripeWebhook stripeWebhook,
    ClaimsChange claimsChange,
    IServiceScopeFactory serviceScopeFactory,
    IEmailService emailSender,
    ILogger<WebhookController> logger,
    IMapper mapper
    ) : Controller
{
    private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

    [HttpPost]
    public async void Index()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        bool lockTaken = false;
        try
        {

            var stripeEvent = stripeWebhook.ConstructEvent(json);
            // ينتظر الحدث الحالي انتهاء أي حدث سابق
            await _lock.WaitAsync();
            lockTaken = true;
            //Log.Information("Type Event is: {0}", stripeEvent.Type);
            switch (stripeEvent.Type)
            {
                case EventTypes.CustomerDeleted:
                    {
                        Customer? customer = stripeEvent.Data.Object as Customer;
                        using var scope = serviceScopeFactory.CreateScope();
                        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        if (customer?.Email == null) break;

                        var user = await userManager.FindByEmailAsync(customer.Email);
                        if (user != null)
                        {
                            user.CustomerId = null;
                            await userManager.UpdateAsync(user);
                        }
                        break;
                    }
                // Handle the event
                case EventTypes.CustomerSubscriptionCreated:
                    {
                        var subscription = stripeEvent.Data.Object as Stripe.Subscription;
                        var price = subscription.Items.First().Price;

                        Console.ForegroundColor = ConsoleColor.Green;
                        logger.LogInformation("Created Subscription: {0}", subscription.Id);
                        Console.WriteLine("Created Subscription: {0}", subscription.Id);

                        await CreateSubscription(subscription, price);
                        claimsChange.SendEmail = true;
                        break;
                    }
                case EventTypes.CustomerSubscriptionUpdated:
                    {
                        var subscription = stripeEvent.Data.Object as Stripe.Subscription;
                        var price = subscription.Items.First().Price;

                        Console.ForegroundColor = ConsoleColor.Green;
                        logger.LogInformation("Updated Subscription: {0}", subscription.Id);
                        Console.WriteLine("Updated Subscription: {0}", subscription.Id);
                        await UpdateSubscription(subscription, price);
                        break;
                    }

                case EventTypes.CustomerSubscriptionDeleted:
                    {
                        var subscription = stripeEvent.Data.Object as Stripe.Subscription;
                        await DeleteSubscription(subscription);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Deleted: {0}", stripeEvent.Type);
                        break;
                    }
                default:
                    // ... handle other event types
                    Console.ForegroundColor = ConsoleColor.Red;
                    //Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    //Log.Information("Unhandled event type: {0}", stripeEvent.Type);
                    break;
            }

        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine(e.Message);
            logger.LogError(e, "Error processing webhook event: {0}", e.Message);
            Console.WriteLine("Error is: {0}", e.Message);
            logger.LogInformation($"Inner Exception is: {e.InnerException?.Message}");
            //throw;
        }
        finally
        {
            // تحرير القفل للسماح للحدث التالي بالعمل
            if (lockTaken) // 🔹 التحقق قبل تحرير القفل
                _lock.Release();
        }
    }

    private async Task CreateSubscription(Stripe.Subscription subscription, Price price)
    {
        using var scope = serviceScopeFactory.CreateScope();
        // Resolve DataContext from the new scope
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var planFeatureService = scope.ServiceProvider.GetRequiredService<IUsePlanFeatureService>();
        //var userService = scope.ServiceProvider.GetRequiredService<IUseApplicationUserService>();
        //var subscriptionService = scope.ServiceProvider.GetRequiredService<IUseSubscriptionService>();

        var user = await context.Users.FirstOrDefaultAsync(u => u.CustomerId == subscription.CustomerId);
        if (user == null)
        {
            logger.LogInformation("User with customer id ({0}) not found ", subscription.CustomerId);
            throw new Exception($"User with customer id {subscription.CustomerId} not found");
        }

        var oldSubscription = await context.Subscriptions.FirstOrDefaultAsync(u => u.UserId == user.Id);

        var newSubscription = new Subscription()
        {
            Id = subscription.Id,
            UserId = user.Id,// store the
            CustomerId = subscription.CustomerId,
            Status = subscription.Status,
            PlanId = price.Id,
            StartDate = subscription.StartDate,
            CurrentPeriodStart = subscription.Items.First().CurrentPeriodStart,
            CurrentPeriodEnd = subscription.Items.First().CurrentPeriodEnd,
        };

        if (oldSubscription != null)
        {
            oldSubscription.UserId = null;
            context.Subscriptions.Update(oldSubscription);
        }
        newSubscription.AllowedRequests = await planFeatureService.GetNumberRequests(price.Id);
        newSubscription.AllowedSpaces = await planFeatureService.GetNumberSpaces(price.Id);

        claimsChange.IsChange = true;
        await context.Subscriptions.AddAsync(newSubscription);
        await context.SaveChangesAsync();

    }

    private async Task UpdateSubscription(Stripe.Subscription subscription, Price price)
    {
        //bool condition = false;
        //while (!condition)
        //{
        using (var scope = serviceScopeFactory.CreateScope())
        {
            // Resolve DataContext from the new scope
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            //var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //var userService = scope.ServiceProvider.GetRequiredService<IUseApplicationUserService>();
            //var subscriptionService = scope.ServiceProvider.GetRequiredService<IUseSubscriptionService>();
            //var planService = scope.ServiceProvider.GetRequiredService<IUsePlanService>();

            var userSubscription = await context.Subscriptions.FirstOrDefaultAsync(s => s.Id == subscription.Id);
            if (userSubscription != null)
            {
                userSubscription.Status = subscription.Status;
                userSubscription.CancelAt = subscription.CancelAt;
                userSubscription.CancelAtPeriodEnd = subscription.CancelAtPeriodEnd;
                userSubscription.CurrentPeriodStart = subscription.Items.First().CurrentPeriodStart;
                userSubscription.CurrentPeriodEnd = subscription.Items.First().CurrentPeriodEnd;
                userSubscription.CanceledAt = subscription.CanceledAt;
                userSubscription.PlanId = price.Id;

                //var subscriptionVM = mapper.Map<SubscriptionOutputVM>(userSubscription);
                var subscriptionRequest = mapper.Map<SubscriptionRequestDso>(userSubscription);
                //await context.Subscriptions.AddRange.UpdateAsync(subscriptionRequest);
                await context.SaveChangesAsync();

                //trackSubscription.RefreshData();
                //trackSubscription.Status = subscription.Status;
                //trackSubscription.CancelAtPeriodEnd = subscription.CancelAtPeriodEnd;

                claimsChange.IsChange = true;
                //claimsChange.User = user;
                //}
                if (claimsChange.SendEmail)
                {
                    var plan = await context.Plans.FindAsync(price.Id);
                    var user = await context.Users.AsNoTracking()
                        .FirstOrDefaultAsync(u => u.CustomerId == subscription.CustomerId);
                    //var namesSelect = plan.pla.Select(p => p.Service.Name);

                    await emailSender.SendSubscriptionSuccessEmailAsync(user?.Email!,
                        user?.DisplayName!,
                        planName: string.Join(", ", plan.ProductName),
                        duration: plan.BillingPeriod,
                        activationDate: subscription.StartDate.ToLongDateString(),
                        subscriptionId: subscription.Id);
                    claimsChange.SendEmail = false;
                }
                //condition = true;
                logger.LogInformation("User Subscription is:{0}", "update subscription successfully");
                //timer.Stop();
            }
            else
            {
                logger.LogInformation("Subscription with id ({0}) not found ", subscription.Id);
            }
        }
        ;
        //} // end while
    }

    private async Task DeleteSubscription(Stripe.Subscription subscription, CancellationToken cancellationToken = default)
    {
        using (var scope = serviceScopeFactory.CreateScope())
        {
            // Resolve DataContext from the new scope

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            //var delete = await context.Subscriptions.Where(us => us.Id == subscription.Id).ExecuteDeleteAsync();
            var subscription1 = await context.Subscriptions.FirstOrDefaultAsync(us => us.Id == subscription.Id);
            if (subscription1 != null)
            {
                subscription1.Status = subscription.Status;
                subscription1.UserId = null;
                context.Subscriptions.Update(subscription1);
                await context.SaveChangesAsync();
                //trackSubscription.Status = SubscriptionStatus.Canceled;
                //trackSubscription.CancelAtPeriodEnd = false;
            }
        }
    }

}
