using System.ComponentModel;

namespace Dto.Auth;

public sealed class ForgotPasswordRequest
{
    /// <summary>
    /// The email address to send the reset password code to if a user with that confirmed email address already exists.
    /// </summary>
    public required string Email { get; init; }
    [DefaultValue("https://localhost:7584/resetPassword")]
    public required string ReturnUrl { get; init; }
}
