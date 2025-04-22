using AutoMapper;
using Dto.Stripe.CheckoutDto;
using Dto.Stripe.Customer;
using LAHJAAPI.V1.Validators;
using LAHJAAPI.V1.Validators.Conditions;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using StripeGateway;
using V1.DyModels.Dso.Requests;
using V1.DyModels.Dso.ResponseFilters;
using V1.DyModels.Dso.Responses;
using V1.Services.Services;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController(
        IUseApplicationUserService userService,
        IStripeSubscription stripeSubscription,
        IStripeCheckout stripeCheckout,
        IStripeCustomer stripeCustomer,
        IUsePlanService planService,
        ILogger<CheckoutController> logger,
        IConditionChecker checker,
        IMapper mapper
        ) : Controller
    {


        [HttpPost(Name = "CreateCheckout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CheckoutResponse>> CreateCheckout(CheckoutOptions checkoutOptions)
        {
            try
            {
                var user = await userService.GetUserWithSubscription();
                if (checker.Check(SubscriptionValidatorStates.IsSubscribe, new SubscriptionResponseFilterDso()))
                {
                    return Conflict(new ProblemDetails { Detail = "You already have subscription" });
                }

                await CreateCustomer(user);

                var plan = await planService.GetByIdAsync(checkoutOptions.PlanId);
                if (plan is null) return NotFound(new ProblemDetails { Title = "NOT FOUND", Detail = "Plan not found" });


                if (plan.Amount == 0)
                {
                    // Create a free subscription
                    return await CreateFreeSubscription(plan.Id, user.CustomerId);
                }

                var options = new SessionCreateOptions
                {
                    Customer = user.CustomerId,
                    SuccessUrl = $"{checkoutOptions.SuccessUrl}?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{checkoutOptions.CancelUrl}",
                    Mode = "subscription",
                    //Locale = "ar",
                    Expand = new List<string> { "customer" },
                    LineItems = new List<SessionLineItemOptions>
                {
                        new SessionLineItemOptions
                        {
                        Price = checkoutOptions.PlanId,
                        Quantity = 1,
                    },
                },
                    // AutomaticTax = new SessionAutomaticTaxOptions { Enabled = true },
                };

                //options.Customer = user.CustomerId;
                //else options.CustomerEmail = user.Email;

                var session = await stripeCheckout.CreateCheckoutSession(options);
                return Ok(new { session.Url });
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Detail = ex.Message });
            }
        }

        private async Task<ActionResult> CreateFreeSubscription(string planId, string customerId)
        {
            logger.LogInformation("Creating free subscription for plan ID: {planId}", planId);
            var sub = await stripeSubscription.CreateAsync(new Stripe.SubscriptionCreateOptions()
            {
                Customer = customerId,
                Items = new List<Stripe.SubscriptionItemOptions>{
                        new Stripe.SubscriptionItemOptions
                        {
                            Price = planId,
                        },
                    },
                TrialPeriodDays = 0, // بدون فترة تجريبية
                PaymentBehavior = "default_incomplete", // يتم تجاهل الدفع لأنه مجاني

            });
            if (sub != null)
            {
                logger.LogInformation("Successfully created free subscription with ID: {subscriptionId}", sub.Id);
                return Ok(new { Message = "You have successfully subscribed to the free plan." });
            }

            logger.LogError("Failed to create free subscription for plan ID: {planId}", planId);
            return BadRequest(new ProblemDetails { Detail = "con not subscribe for free plan" });
        }
        private async Task CreateCustomer(ApplicationUserResponseDso user)
        {
            try
            {
                if (user.CustomerId != null) return;
                logger.LogInformation("Creating customer for user: {@user}", user);
                var customers = await stripeCustomer.GetCustomersByEmail(user.Email);
                var customer = customers.FirstOrDefault();
                if (customer == null)
                {
                    customer = await stripeCustomer.CreateAsync(new Stripe.CustomerCreateOptions()
                    {
                        Name = user.DisplayName,
                        Email = user.Email
                    });

                    user.CustomerId = customer.Id;
                    //await userManager.AddClaimAsync(user, new Claim(ClaimTypes2.CustomerId, customer.Id));
                }
                else
                {
                    user.CustomerId = customer.Id;
                }
                //var userVM = mapper.Map<ApplicationUserInfoVM>(user);
                var userRequest = mapper.Map<ApplicationUserRequestDso>(user);
                logger.LogInformation("Updating user with new customer ID: {customerId}", user.CustomerId);
                await userService.UpdateAsync(userRequest);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while creating customer for user: {@user}", user);
                throw;
            }
        }


        [HttpPost("manage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CheckoutResponse>> ManageSubscription(SessionCreate sessionCreate)
        {
            try
            {
                var user = await userService.GetUser();
                //if(user.CustomerId==null) return 
                var session = await stripeCustomer.CustomerPortal(new Stripe.BillingPortal.SessionCreateOptions
                {
                    Customer = user.CustomerId,
                    ReturnUrl = sessionCreate.ReturnUrl,
                    //Locale = "ar"
                });
                return Ok(new { session.Url });
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Detail = ex.Message });
            }
        }
    }
}