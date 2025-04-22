using AutoGenerator.Helper.Translation;
using LAHJAAPI.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StripeGateway;
using V1.DyModels.Dso.Requests;
using V1.DyModels.Dso.Responses;
using V1.DyModels.VMs;
using V1.Services.Services;

namespace V1.Controllers.Api
{
    //[ApiExplorerSettings(GroupName = "V1")]
    [Route("api/V1/Api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IUseSubscriptionService _subscriptionService;
        private readonly IUseApplicationUserService _userService;
        private readonly IStripeCustomer _stripeCustomer;
        private readonly IStripeSubscription _stripeSubscription;
        private readonly IUsePlanService _planService;
        private readonly IUsePlanFeatureService _planFeatureService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public SubscriptionController(
            IUseSubscriptionService subscriptionService,
            IUseApplicationUserService userService,
            IStripeCustomer stripeCustomer,
            IStripeSubscription stripeSubscription,
            IUsePlanService planService,
            IUsePlanFeatureService planFeatureService,
            IMapper mapper,
            ILoggerFactory logger)
        {
            _subscriptionService = subscriptionService;
            _userService = userService;
            _stripeCustomer = stripeCustomer;
            _stripeSubscription = stripeSubscription;
            _planService = planService;
            _planFeatureService = planFeatureService;
            _mapper = mapper;
            _logger = logger.CreateLogger(typeof(SubscriptionController).FullName);
        }

        // Get all Subscriptions.
        [HttpGet(Name = "GetSubscriptions")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<SubscriptionOutputVM>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all Subscriptions...");
                var result = await _subscriptionService.GetAllAsync();
                //var subscriptionRequest = _mapper.Map<List<SubscriptionRequestDso>>(result);

                var items = _mapper.Map<List<SubscriptionOutputVM>>(result);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all Subscriptions");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Get a Subscription by ID.
        [HttpGet("{id}", Name = "GetSubscription")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SubscriptionOutputVM>> GetById(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid Subscription ID received.");
                return BadRequest("Invalid Subscription ID.");
            }

            try
            {
                _logger.LogInformation("Fetching Subscription with ID: {id}", id);
                var entity = await _subscriptionService.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Subscription not found with ID: {id}", id);
                    return NotFound();
                }

                var item = _mapper.Map<SubscriptionOutputVM>(entity);
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching Subscription with ID: {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // // Get a Subscription by Lg.
        [HttpGet("GetSubscriptionByLanguage", Name = "GetSubscriptionByLg")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SubscriptionOutputVM>> GetSubscriptionByLg(SubscriptionFilterVM model)
        {
            var id = model.Id;
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid Subscription ID received.");
                return BadRequest("Invalid Subscription ID.");
            }

            try
            {
                _logger.LogInformation("Fetching Subscription with ID: {id}", id);
                var entity = await _subscriptionService.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Subscription not found with ID: {id}", id);
                    return NotFound();
                }

                var item = _mapper.Map<SubscriptionOutputVM>(entity, opt => opt.Items.Add(HelperTranslation.KEYLG, model.Lg));
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching Subscription with ID: {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // // Get a Subscriptions by Lg.
        [HttpGet("GetSubscriptionsByLanguage", Name = "GetSubscriptionsByLg")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<SubscriptionOutputVM>>> GetSubscriptionsByLg(string? lg)
        {
            if (string.IsNullOrWhiteSpace(lg))
            {
                _logger.LogWarning("Invalid Subscription lg received.");
                return BadRequest("Invalid Subscription lg null ");
            }

            try
            {
                var subscriptions = await _subscriptionService.GetAllAsync();
                if (subscriptions == null)
                {
                    _logger.LogWarning("Subscriptions not found  by  ");
                    return NotFound();
                }

                var items = _mapper.Map<IEnumerable<SubscriptionOutputVM>>(subscriptions, opt => opt.Items.Add(HelperTranslation.KEYLG, lg));
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching Subscriptions with Lg: {lg}", lg);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost(Name = "CreateSubscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<SubscriptionInfoVM>> CreateSubscription(SubscriptionCreateVM subscriptionCreate)
        {
            var user = await _userService.GetUserWithSubscription();

            if (user.CustomerId == null)
            {
                await CreateCustomer(user);
            }

            if (user.Subscription == null)
            {
                _logger.LogInformation("Creating new Subscription with data: {@subscriptionCreate}", subscriptionCreate);
                var plan = await _planService.GetByIdAsync(subscriptionCreate.PlanId);
                if (plan is null)
                {
                    _logger.LogWarning("Plan not found with ID: {planId}", subscriptionCreate.PlanId);
                    return NotFound(HandelErrors.NotFound("Plan not found"));
                }

                if (plan.Amount == 0)
                {
                    // Create a free subscription
                    return await CreateFreeSubscription(plan.Id, user.CustomerId);
                }


                // Automatically save the payment method to the subscription
                // when the first payment is successful.
                var paymentSettings = new Stripe.SubscriptionPaymentSettingsOptions
                {
                    SaveDefaultPaymentMethod = "on_subscription",
                };

                // Create the subscription. Note we're expanding the Subscription's
                // latest invoice and that invoice's payment_intent
                // so we can pass it to the front end to confirm the payment
                try
                {
                    var subscriptionOptions = new Stripe.SubscriptionCreateOptions
                    {
                        Customer = user.CustomerId,
                        Items = new List<Stripe.SubscriptionItemOptions>
                {
                    new () { Price = plan.Id},
                },
                        PaymentSettings = paymentSettings,
                        PaymentBehavior = "default_incomplete",
                    };
                    subscriptionOptions.AddExpand("latest_invoice.payment_intent");

                    var subscription = await _stripeSubscription.CreateAsync(subscriptionOptions);

                    return new SubscriptionInfoVM
                    {
                        SubscriptionId = subscription.Id,
                        //ClientSecret = subscription.LatestInvoice.PaymentSettings..ClientSecret,
                    };
                }
                catch (Stripe.StripeException e)
                {
                    _logger.LogError(e, "Error while creating subscription");
                    return BadRequest($"Failed to create subscription.{e}");
                }
            }
            else if (user.Subscription.Status == "incomplete")
            {
                Stripe.SubscriptionGetOptions options = new()
                {
                    Expand = new List<string> { "latest_invoice.payment_intent" }
                };
                var subscription = await _stripeSubscription.GetByIdAsync(user.Subscription.Id, options);
                return new SubscriptionInfoVM
                {
                    SubscriptionId = subscription.Id,
                    //ClientSecret = subscription.LatestInvoice.PaymentIntent.ClientSecret,
                };
            }
            else
            {
                return Conflict(new ProblemDetails { Detail = "You already have subscription" });
            }


        }



        [HttpPut("PauseCollection/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PauseCollection(string id, SubscriptionUpdateRequest subscriptionUpdate)
        {
            try
            {
                var result = await _stripeSubscription.UpdateAsync(id, new Stripe.SubscriptionUpdateOptions
                {
                    PauseCollection = new Stripe.SubscriptionPauseCollectionOptions()
                    {
                        Behavior = subscriptionUpdate.PauseCollectionBehavior.ToString(),
                        ResumesAt = subscriptionUpdate.ResumesAt
                    }
                });

                //if (result.CancelAtPeriodEnd)
                return Ok();
                //return BadRequest(new ProblemDetails { Detail = "Faild cancel subscription" });
            }
            catch (Exception ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
        }


        [HttpPut("ResumeCollection/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResumeCollection(string id)
        {
            try
            {
                var options = new Stripe.SubscriptionUpdateOptions();
                options.AddExtraParam("pause_collection", "");

                var result = await _stripeSubscription.UpdateAsync(id, options);
                //var item = await subscriptionRepository.GetByIdAsync(id);
                return Ok();
            }
            catch (Stripe.StripeException ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
            catch (Exception ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
        }



        [HttpDelete("cancel/{id}", Name = "CancelSubscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelSubscription(string id)
        {
            try
            {
                var result = await _stripeSubscription.CancelAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
        }

        [HttpPut("CancelAtEnd/{id}", Name = "CancelAtEnd")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelAtEnd(string id)
        {
            try
            {
                var result = await _stripeSubscription.UpdateAsync(id, new Stripe.SubscriptionUpdateOptions
                {
                    CancelAtPeriodEnd = true
                });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
        }

        [HttpPut("Renew/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Renew(string id)
        {
            try
            {
                var result = await _stripeSubscription.UpdateAsync(id, new Stripe.SubscriptionUpdateOptions
                {
                    CancelAtPeriodEnd = false
                });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
        }

        [HttpPut("resume/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Resume(string id, SubscriptionResumeRequest subscriptionResume)
        {
            try
            {
                var result = await _stripeSubscription.ResumeAsync(id, new Stripe.SubscriptionResumeOptions
                {
                    BillingCycleAnchor = Stripe.SubscriptionBillingCycleAnchor.Now, // إعادة ضبط تاريخ الفوترة
                    ProrationBehavior = subscriptionResume.ProrationBehavior
                });
                //var item = await subscriptionRepository.GetByIdAsync(id);
                return Ok();
            }
            catch (Stripe.StripeException ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
            catch (Exception ex)
            {
                return BadRequest(HandelErrors.Problem(ex));
            }
        }


        [HttpPut("resetRequests/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetRequests(string id)
        {
            try
            {
                _logger.LogInformation("Resetting requests for subscription with ID: {id}", id);
                var subscription = await _subscriptionService.GetByIdAsync(id);
                subscription.AllowedRequests = await _planFeatureService.GetNumberRequests(subscription.PlanId);
                var subscriptionVM = _mapper.Map<SubscriptionOutputVM>(subscription);
                await _subscriptionService.UpdateAsync(_mapper.Map<SubscriptionRequestDso>(subscriptionVM));
                return Ok();
            }
            catch (Stripe.StripeException ex)
            {
                _logger.LogError(ex, "Error while resetting requests for subscription with ID: {id}", id);
                return BadRequest(HandelErrors.Problem(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while resetting requests for subscription with ID: {id}", id);
                return BadRequest(HandelErrors.Problem(ex));
            }
        }


        [HttpPut("resetSpaces/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetSpaces(string id)
        {
            try
            {
                _logger.LogInformation("Resetting spaces for subscription with ID: {id}", id);
                var subscription = await _subscriptionService.GetByIdAsync(id);
                subscription.AllowedSpaces = await _planFeatureService.GetNumberSpaces(subscription.PlanId);
                var subscriptionVM = _mapper.Map<SubscriptionOutputVM>(subscription);
                await _subscriptionService.UpdateAsync(_mapper.Map<SubscriptionRequestDso>(subscriptionVM));
                return Ok();
            }
            catch (Stripe.StripeException ex)
            {
                _logger.LogError(ex, "Error while resetting spaces for subscription with ID: {id}", id);
                return BadRequest(HandelErrors.Problem(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while resetting spaces for subscription with ID: {id}", id);
                return BadRequest(HandelErrors.Problem(ex));
            }
        }




        //[ApiExplorerSettings(IgnoreApi = true)]
        //[HttpDelete("DeleteByStatus/{status}", Name = "DeleteByStatus")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> DeleteByStatus(string status)
        //{
        //    try
        //    {
        //        await _subscriptionService.DeleteAllAsync(s => s.Status == status);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(HandelErrors.Problem(ex));
        //    }
        //}


        private async Task<ActionResult> CreateFreeSubscription(string planId, string customerId)
        {
            _logger.LogInformation("Creating free subscription for plan ID: {planId}", planId);
            var sub = await _stripeSubscription.CreateAsync(new Stripe.SubscriptionCreateOptions()
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
                _logger.LogInformation("Successfully created free subscription with ID: {subscriptionId}", sub.Id);
                return Ok(new { Message = "You have successfully subscribed to the free plan." });
            }

            _logger.LogError("Failed to create free subscription for plan ID: {planId}", planId);
            return BadRequest(new ProblemDetails { Detail = "con not subscribe for free plan" });
        }


        private async Task CreateCustomer(ApplicationUserResponseDso user)
        {
            try
            {
                _logger.LogInformation("Creating customer for user: {@user}", user);
                var customers = await _stripeCustomer.GetCustomersByEmail(user.Email);
                var customer = customers.FirstOrDefault();
                if (customer == null)
                {
                    customer = await _stripeCustomer.CreateAsync(new Stripe.CustomerCreateOptions()
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
                var userVM = _mapper.Map<ApplicationUserInfoVM>(user);
                var userRequest = _mapper.Map<ApplicationUserRequestDso>(userVM);
                _logger.LogInformation("Updating user with new customer ID: {customerId}", user.CustomerId);
                await _userService.UpdateAsync(userRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating customer for user: {@user}", user);
                throw;
            }
        }


        // Update an existing Subscription.
        [HttpPut(Name = "UpdateSubscription")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SubscriptionOutputVM>> Update([FromBody] SubscriptionUpdateVM model)
        {

            try
            {
                _logger.LogInformation("Updating Subscription with ID: {id}", model?.Id);
                var item = _mapper.Map<SubscriptionRequestDso>(model);
                var updatedEntity = await _subscriptionService.UpdateAsync(item);
                if (updatedEntity == null)
                {
                    _logger.LogWarning("Subscription not found for update with ID: {id}", model?.Id);
                    return NotFound();
                }

                var updatedItem = _mapper.Map<SubscriptionOutputVM>(updatedEntity);
                return Ok(updatedItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating Subscription with ID: {id}", model?.Id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Delete a Subscription.
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpDelete("{id}", Name = "DeleteSubscription")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid Subscription ID received in Delete.");
                return BadRequest("Invalid Subscription ID.");
            }

            try
            {
                _logger.LogInformation("Deleting Subscription with ID: {id}", id);
                await _subscriptionService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting Subscription with ID: {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Get count of Subscriptions.
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("CountSubscription")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> Count()
        {
            try
            {
                _logger.LogInformation("Counting Subscriptions...");
                var count = await _subscriptionService.CountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while counting Subscriptions");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}