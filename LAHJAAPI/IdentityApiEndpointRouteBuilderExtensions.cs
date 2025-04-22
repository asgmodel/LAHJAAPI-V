// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoBuilderApiCore.Auth;
using LAHJAAPI.Services2;
using LAHJAAPI.Models;
using LAHJAAPI.Utilities;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
namespace Api;

/// <summary>
/// Provides extension methods for <see cref="IEndpointRouteBuilder"/> to add identity endpoints.
/// </summary>
public static class IdentityApiEndpointRouteBuilderExtensions
{
    // Validate the email address using DataAnnotations like the UserValidator does when RequireUniqueEmail = true.
    private static readonly EmailAddressAttribute _emailAddressAttribute = new();

    /// <summary>
    /// Add endpoints for registering, logging in, and logging out using ASP.NET Core Identity.
    /// </summary>
    /// <typeparam name="TUser">The type describing the user. This should match the generic parameter in <see cref="UserManager{TUser}"/>.</typeparam>
    /// <param name="endpoints">
    /// The <see cref="IEndpointRouteBuilder"/> to add the identity endpoints to.
    /// Call <see cref="EndpointRouteBuilderExtensions.MapGroup(IEndpointRouteBuilder, string)"/> to add a prefix to all the endpoints.
    /// </param>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> to further customize the added endpoints.</returns>
    public static IEndpointConventionBuilder CustomMapIdentityApi<TUser>(this IEndpointRouteBuilder endpoints)
        where TUser : class, new()
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var timeProvider = endpoints.ServiceProvider.GetRequiredService<TimeProvider>();
        var bearerTokenOptions = endpoints.ServiceProvider.GetRequiredService<IOptionsMonitor<BearerTokenOptions>>();
        var emailSender = endpoints.ServiceProvider.GetRequiredService<IEmailSender<TUser>>();
        var linkGenerator = endpoints.ServiceProvider.GetRequiredService<LinkGenerator>();
        var appSettings = endpoints.ServiceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
        //var tokenListService = endpoints.ServiceProvider.GetRequiredService<TokenlistService>();

        // We'll figure out a unique endpoint name based on the final route pattern during endpoint generation.
        string? confirmEmailEndpointName = null;

        var routeGroup = endpoints.MapGroup("api");

        // NOTE: We cannot inject UserManager<TUser> directly because the TUser generic parameter is currently unsupported by RDG.
        // https://github.com/dotnet/aspnetcore/issues/47338
        routeGroup.MapPost("/register", async Task<Results<Ok, ValidationProblem,
            BadRequest<Dictionary<string, Dictionary<string, List<string>>>>>>
            ([FromBody] Dto.Auth.RegisterRequest registration, HttpContext context, [FromServices] IServiceProvider sp) =>
        {
            var dict = Validate(registration);
            if (dict != null) return TypedResults.BadRequest(dict);

            var userManager = sp.GetRequiredService<UserManager<TUser>>();

            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException($"{nameof(CustomMapIdentityApi)} requires a user store with email support.");
            }

            var userStore = sp.GetRequiredService<IUserStore<TUser>>();
            var emailStore = (IUserEmailStore<TUser>)userStore;
            var PhoneNumberStore = (IUserPhoneNumberStore<TUser>)userStore;
            //var ClaimStore = (IUserClaimStore<TUser>)userStore;

            var email = registration.Email;
            var phoneNumber = registration.PhoneNumber;

            if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
            {
                return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
            }

            var user = new TUser();
            await userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            await PhoneNumberStore.SetPhoneNumberAsync(user, phoneNumber, CancellationToken.None);

            var appUser = user as ApplicationUser;
            appUser!.FirstName = registration.FirsName;
            appUser.LastName = registration.LastName;
            appUser.DisplayName = appUser.FirstName + " " + appUser.LastName;
            appUser.Image = registration.Avatar;

            user = appUser as TUser;
            //var newUser = await userStore.GetUserNameAsync(user, email, CancellationToken.None);



            var result = await userManager.CreateAsync(user, registration.Password);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            await SendConfirmationEmailAsync(user, userManager, context, registration.ReturnUrl, email);

            return TypedResults.Ok();
        }).WithTags("Auth");

        routeGroup.MapPost("/login", async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult,
            BadRequest<Dictionary<string, Dictionary<string, List<string>>>>>>
            ([FromBody] Dto.Auth.LoginRequest login, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies, [FromServices] IServiceProvider sp) =>
        {
            var validate = Validate(login);
            if (validate != null) return TypedResults.BadRequest(validate);
            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
            var signInManager = sp.GetRequiredService<SignInManager<ApplicationUser>>();

            var useCookieScheme = useCookies == true || useSessionCookies == true;
            var isPersistent = useCookies == true && useSessionCookies != true;
            signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

            var user = await userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                return TypedResults.Problem("Your email is incorrect", statusCode: StatusCodes.Status401Unauthorized);
            }

            if (user.IsArchived)
            {
                return TypedResults.Problem("Your account is suspended.", statusCode: StatusCodes.Status401Unauthorized);

                //return TypedResults.Problem(SignInResult.Failed.ToString(), statusCode: StatusCodes.Status401Unauthorized);

            }

            //await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow); // ينهي القفل فورًا
            //await userManager.ResetAccessFailedCountAsync(user); // إعادة تعيين عداد المحاولات

            var result = await signInManager.PasswordSignInAsync(user, login.Password, isPersistent, lockoutOnFailure: true);

            if (result.RequiresTwoFactor)
            {
                if (!string.IsNullOrEmpty(login.TwoFactorCode))
                {
                    result = await signInManager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode, isPersistent, rememberClient: isPersistent);
                }
                else if (!string.IsNullOrEmpty(login.TwoFactorRecoveryCode))
                {
                    result = await signInManager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode);
                }
            }

            if (result.IsNotAllowed)
            {
                return TypedResults.Problem("You must confirm your email first", statusCode: StatusCodes.Status401Unauthorized);
            }
            if (result.IsLockedOut)
            {
                return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status423Locked);
            }
            if (!result.Succeeded)
            {
                return TypedResults.Problem("Email or password are incorrect", statusCode: StatusCodes.Status401Unauthorized);
                //return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
            }

            return TypedResults.Empty;
        }).WithTags("Auth");

        routeGroup.MapPost("/refresh", async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult, SignInHttpResult, ChallengeHttpResult>>
            ([FromBody] RefreshRequest refreshRequest, [FromServices] IServiceProvider sp) =>
        {
            var signInManager = sp.GetRequiredService<SignInManager<TUser>>();
            var refreshTokenProtector = bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
            var refreshTicket = refreshTokenProtector.Unprotect(refreshRequest.RefreshToken);

            // Reject the /refresh attempt with a 401 if the token expired or the security stamp validation fails
            if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
                timeProvider.GetUtcNow() >= expiresUtc ||
                await signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not TUser user)

            {
                return TypedResults.Challenge();
            }

            var newPrincipal = await signInManager.CreateUserPrincipalAsync(user);
            return TypedResults.SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);
        }).WithTags("Auth");

        routeGroup.MapPost("/confirmEmail", async Task<Results<Ok, ContentHttpResult, UnauthorizedHttpResult>>
            ([FromBody] ConfirmEmailRequest confirmEmail, [FromServices] IServiceProvider sp) =>
        {
            var userManager = sp.GetRequiredService<UserManager<TUser>>();
            //var tokenListService = sp.GetRequiredService<TokenListService>();
            if (await userManager.FindByEmailAsync(confirmEmail.Email) is not { } user)
            {
                // We could respond with a 404 instead of a 401 like Identity UI, but that feels like unnecessary information.
                return TypedResults.Unauthorized();
            }

            if (await userManager.IsEmailConfirmedAsync(user))
            {
                return TypedResults.Ok();
            }

            try
            {
                confirmEmail.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmEmail.Code));
            }
            catch (FormatException)
            {
                return TypedResults.Unauthorized();
            }

            IdentityResult result;

            if (string.IsNullOrEmpty(confirmEmail.ChangedEmail) || !confirmEmail.IsChangedEmail)
            {
                result = await userManager.ConfirmEmailAsync(user, confirmEmail.Code);
            }
            else
            {
                // As with Identity UI, email and user name are one and the same. So when we update the email,
                // we need to update the user name.
                result = await userManager.ChangeEmailAsync(user, confirmEmail.ChangedEmail, confirmEmail.Code);

                if (result.Succeeded)
                {
                    result = await userManager.SetUserNameAsync(user, confirmEmail.ChangedEmail);
                }
            }

            if (!result.Succeeded)
            {
                return TypedResults.Unauthorized();
            }

            return TypedResults.Ok();
            //return TypedResults.Text("Thank you for confirming your email.");
        }).WithTags("Auth")
        .Add(endpointBuilder =>
        {
            var finalPattern = ((RouteEndpointBuilder)endpointBuilder).RoutePattern.RawText;
            confirmEmailEndpointName = $"{nameof(CustomMapIdentityApi)}-{finalPattern}";
            endpointBuilder.Metadata.Add(new EndpointNameMetadata(confirmEmailEndpointName));
        });

        routeGroup.MapPost("/resendConfirmationEmail", async Task<Results<Ok, Ok<string>, ValidationProblem>>
            ([FromBody] Dto.Auth.ResendConfirmationEmailRequest resendRequest, HttpContext context, [FromServices] IServiceProvider sp, [FromServices] TokenListService tokenListService) =>
        {
            var userManager = sp.GetRequiredService<UserManager<TUser>>();
            if (await userManager.FindByEmailAsync(resendRequest.Email) is not { } user)
            {
                return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(resendRequest.Email)));
            }
            if (await userManager.IsEmailConfirmedAsync(user))
            {
                return TypedResults.Ok<string>("Your email has already been confirmed.");
            }
            await SendConfirmationEmailAsync(user, userManager, context, resendRequest.ReturnUrl, resendRequest.Email);
            return TypedResults.Ok();
        }).WithTags("Auth");

        routeGroup.MapPost("/forgotPassword", async Task<Results<Ok, ValidationProblem>>
            ([FromBody] Dto.Auth.ForgotPasswordRequest resetRequest, [FromServices] IServiceProvider sp) =>
        {
            var userManager = sp.GetRequiredService<UserManager<TUser>>();
            var user = await userManager.FindByEmailAsync(resetRequest.Email);

            if (user is not null && await userManager.IsEmailConfirmedAsync(user))
            {
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                string resetLink = $"{resetRequest.ReturnUrl}?code={code}";

                await emailSender.SendPasswordResetLinkAsync(user, resetRequest.Email, HtmlEncoder.Default.Encode(resetLink));
                //tokenListService.AddTokenToList(code, DateTime.UtcNow.AddMinutes(1));
                return TypedResults.Ok();
            }

            // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
            // returned a 400 for an invalid code given a valid user email.
            return CreateValidationProblem(StatusCodes.Status400BadRequest.ToString(), "Incorrect your email or not confirmed");
        }).WithTags("Auth");

        routeGroup.MapPost("/resetPassword", async Task<Results<Ok, ValidationProblem>>
            ([FromBody] ResetPasswordRequest resetRequest, [FromServices] IServiceProvider sp) =>
        {
            var userManager = sp.GetRequiredService<UserManager<TUser>>();
            var tokenListService = sp.GetRequiredService<TokenListService>();

            var user = await userManager.FindByEmailAsync(resetRequest.Email);

            if (user is null || !await userManager.IsEmailConfirmedAsync(user))
            {
                // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
                // returned a 400 for an invalid code given a valid user email.
                return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken()));
            }
            //if (!tokenListService.IsTokenListed(resetRequest.ResetCode))
            //{
            //    return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken()));
            //}
            IdentityResult result;
            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetRequest.ResetCode));
                result = await userManager.ResetPasswordAsync(user, code, resetRequest.NewPassword);
            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken());
            }

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        })
            .WithTags("Auth");

        var accountGroup = routeGroup.MapGroup("/manage").RequireAuthorization().WithTags("Manage");

        accountGroup.MapPost("/twofa", async Task<Results<Ok<TwoFactorResponse>, ValidationProblem, NotFound>>
            (ClaimsPrincipal claimsPrincipal, [FromBody] TwoFactorRequest tfaRequest, [FromServices] IServiceProvider sp) =>
        {
            var signInManager = sp.GetRequiredService<SignInManager<TUser>>();
            var userManager = signInManager.UserManager;
            if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
            {
                return TypedResults.NotFound();
            }

            if (tfaRequest.Enable == true)
            {
                if (tfaRequest.ResetSharedKey)
                {
                    return CreateValidationProblem("CannotResetSharedKeyAndEnable",
                        "Resetting the 2fa shared key must disable 2fa until a 2fa token based on the new shared key is validated.");
                }
                else if (string.IsNullOrEmpty(tfaRequest.TwoFactorCode))
                {
                    return CreateValidationProblem("RequiresTwoFactor",
                        "No 2fa token was provided by the request. A valid 2fa token is required to enable 2fa.");
                }
                else if (!await userManager.VerifyTwoFactorTokenAsync(user, userManager.Options.Tokens.AuthenticatorTokenProvider, tfaRequest.TwoFactorCode))
                {
                    return CreateValidationProblem("InvalidTwoFactorCode",
                        "The 2fa token provided by the request was invalid. A valid 2fa token is required to enable 2fa.");
                }

                await userManager.SetTwoFactorEnabledAsync(user, true);
            }
            else if (tfaRequest.Enable == false || tfaRequest.ResetSharedKey)
            {
                await userManager.SetTwoFactorEnabledAsync(user, false);
            }

            if (tfaRequest.ResetSharedKey)
            {
                await userManager.ResetAuthenticatorKeyAsync(user);
            }

            string[]? recoveryCodes = null;
            if (tfaRequest.ResetRecoveryCodes || tfaRequest.Enable == true && await userManager.CountRecoveryCodesAsync(user) == 0)
            {
                var recoveryCodesEnumerable = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                recoveryCodes = recoveryCodesEnumerable?.ToArray();
            }

            if (tfaRequest.ForgetMachine)
            {
                await signInManager.ForgetTwoFactorClientAsync();
            }

            var key = await userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(key))
            {
                await userManager.ResetAuthenticatorKeyAsync(user);
                key = await userManager.GetAuthenticatorKeyAsync(user);

                if (string.IsNullOrEmpty(key))
                {
                    throw new NotSupportedException("The user manager must produce an authenticator key after reset.");
                }
            }

            return TypedResults.Ok(new TwoFactorResponse
            {
                SharedKey = key,
                RecoveryCodes = recoveryCodes,
                RecoveryCodesLeft = recoveryCodes?.Length ?? await userManager.CountRecoveryCodesAsync(user),
                IsTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user),
                IsMachineRemembered = await signInManager.IsTwoFactorClientRememberedAsync(user),
            });
        });

        accountGroup.MapGet("/info", async Task<Results<Ok<InfoResponse>, ValidationProblem, NotFound>>
            (ClaimsPrincipal claimsPrincipal, [FromServices] IServiceProvider sp) =>
        {
            var userManager = sp.GetRequiredService<UserManager<TUser>>();
            if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(await CreateInfoResponseAsync(user, userManager));
        });

        accountGroup.MapPost("/info", async Task<Results<Ok<InfoResponse>, ValidationProblem, NotFound>>
            (ClaimsPrincipal claimsPrincipal, [FromBody] Dto.Auth.InfoRequest infoRequest, HttpContext context, [FromServices] IServiceProvider sp) =>
        {
            var userManager = sp.GetRequiredService<UserManager<TUser>>();
            if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
            {
                return TypedResults.NotFound();
            }

            if (!string.IsNullOrEmpty(infoRequest.NewEmail) && !_emailAddressAttribute.IsValid(infoRequest.NewEmail))
            {
                return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(infoRequest.NewEmail)));
            }

            if (!string.IsNullOrEmpty(infoRequest.NewPassword))
            {
                if (string.IsNullOrEmpty(infoRequest.OldPassword))
                {
                    return CreateValidationProblem("OldPasswordRequired",
                        "The old password is required to set a new password. If the old password is forgotten, use /resetPassword.");
                }

                var changePasswordResult = await userManager.ChangePasswordAsync(user, infoRequest.OldPassword, infoRequest.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    return CreateValidationProblem(changePasswordResult);
                }
            }

            if (!string.IsNullOrEmpty(infoRequest.NewEmail))
            {
                var email = await userManager.GetEmailAsync(user);

                if (email != infoRequest.NewEmail)
                {
                    await SendConfirmationEmailAsync(user, userManager, context, infoRequest.ReturnUrl, infoRequest.NewEmail, isChange: true);
                }
            }

            return TypedResults.Ok(await CreateInfoResponseAsync(user, userManager));
        });

        async Task SendConfirmationEmailAsync(TUser user, UserManager<TUser> userManager, HttpContext context, string returnUrl, string email, bool isChange = false)
        {
            //if (confirmEmailEndpointName is null)
            //{
            //    throw new NotSupportedException("No email confirmation endpoint was registered!");
            //}

            var code = isChange
                ? await userManager.GenerateChangeEmailTokenAsync(user, email)
                : await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            //var userId = await userManager.GetEmailAsync(user);
            var routeValues = new RouteValueDictionary()
            {
                ["email"] = email,
                ["code"] = code,
            };


            if (isChange)
            {
                // This is validated by the /confirmEmail endpoint on change.
                routeValues.Add("changedEmail", email);
            }

            //var confirmEmailUrl = linkGenerator.GetUriByName(context,
            //                                                 confirmEmailEndpointName,
            //                                                 routeValues,
            //                                                 host: new HostString(appSettings.Client),
            //                                                 pathBase: new PathString("/confirm-email"))
            //?? throw new NotSupportedException($"Could not find endpoint named '{confirmEmailEndpointName}'.");
            string queryString = string.Join("&", routeValues.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value.ToString())}"));

            string confirmEmailUrl = $"{returnUrl}?{queryString}";
            await emailSender.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(confirmEmailUrl));
            //tokenListService.AddTokenToList(code, DateTime.UtcNow.AddMinutes(10));
        }

        return new IdentityEndpointsConventionBuilder(routeGroup);
    }


    private static Dictionary<string, Dictionary<string, List<string>>> Validate(object registration)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(registration);
        if (!Validator.TryValidateObject(registration, validationContext, validationResults, true))
        {
            var dict = new Dictionary<string, List<string>>();
            var errorDictionary = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (var error in validationResults)
            {
                dict.Add(error.MemberNames.First(), [error.ErrorMessage]);
            }

            errorDictionary.Add("errors", dict);
            return errorDictionary;
        }
        return null;
    }

    private static ValidationProblem CreateValidationProblem(string errorCode, string errorDescription) =>
        TypedResults.ValidationProblem(new Dictionary<string, string[]> {
            { errorCode, [errorDescription] }
        });

    private static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        // We expect a single error code and description in the normal case.
        // This could be golfed with GroupBy and ToDictionary, but perf! :P
        Debug.Assert(!result.Succeeded);
        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (var error in result.Errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(error.Code, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = [error.Description];
            }

            errorDictionary[error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }

    private static async Task<InfoResponse> CreateInfoResponseAsync<TUser>(TUser user, UserManager<TUser> userManager)
        where TUser : class
    {
        return new()
        {
            Email = await userManager.GetEmailAsync(user) ?? throw new NotSupportedException("Users must have an email."),
            IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user),
        };
    }

    // Wrap RouteGroupBuilder with a non-public type to avoid a potential future behavioral breaking change.
    private sealed class IdentityEndpointsConventionBuilder(RouteGroupBuilder inner) : IEndpointConventionBuilder
    {
        private IEndpointConventionBuilder InnerAsConventionBuilder => inner;

        public void Add(Action<EndpointBuilder> convention) => InnerAsConventionBuilder.Add(convention);
        public void Finally(Action<EndpointBuilder> finallyConvention) => InnerAsConventionBuilder.Finally(finallyConvention);
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    private sealed class FromBodyAttribute : Attribute, IFromBodyMetadata
    {
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    private sealed class FromServicesAttribute : Attribute, IFromServiceMetadata
    {
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    private sealed class FromQueryAttribute : Attribute, IFromQueryMetadata
    {
        public string? Name => null;
    }
}