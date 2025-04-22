using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dto.Auth;

public sealed class LoginRequest
{
    /// <summary>
    /// The user's email address which acts as a user name.
    /// </summary>

    [Required(ErrorMessage = "The Email field is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [DefaultValue("admin@gmail.com")]
    public string Email { get; init; }

    /// <summary>
    /// The user's password.
    /// </summary>

    [Required]
    [DataType(DataType.Password)]
    [DefaultValue("Admin123*")]
    public string Password { get; init; }


    //[Required]
    //[DefaultValue("price_free_plan")]
    //public string PlanId { get; init; }

    /// <summary>
    /// The optional two-factor authenticator code. This may be required for users who have enabled two-factor authentication.
    /// This is not required if a <see cref="TwoFactorRecoveryCode"/> is sent.
    /// </summary>
    public string? TwoFactorCode { get; init; }

    /// <summary>
    /// An optional two-factor recovery code from <see cref="TwoFactorResponse.RecoveryCodes"/>.
    /// This is required for users who have enabled two-factor authentication but lost access to their <see cref="TwoFactorCode"/>.
    /// </summary>
    public string? TwoFactorRecoveryCode { get; init; }

}
