using System.ComponentModel;

namespace Dto.Auth
{
    public sealed class ResendConfirmationEmailRequest
    {
        //
        // Summary:
        //     The email address to resend the confirmation email to if a user with that email
        //     exists.
        public required string Email { get; init; }
        [DefaultValue("https://localhost:7584/confirm-email")]
        public required string ReturnUrl { get; init; }
    }
}
