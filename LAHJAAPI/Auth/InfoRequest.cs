using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dto.Auth
{
    //
    // Summary:
    //     The request type for the "/manage/info" endpoint added by Microsoft.AspNetCore.Routing.IdentityApiEndpointRouteBuilderExtensions.MapIdentityApi``1(Microsoft.AspNetCore.Routing.IEndpointRouteBuilder).
    //     All properties are optional. No modifications will be made to the user if all
    //     the properties are omitted from the request.
    public sealed class InfoRequest
    {
        //
        // Summary:
        //     The optional new email address for the authenticated user. This will replace
        //     the old email address if there was one. The email will not be updated until it
        //     is confirmed.
        public string? NewEmail { get; init; }
        //
        // Summary:
        //     The optional new password for the authenticated user. If a new password is provided,
        //     the Microsoft.AspNetCore.Identity.Data.InfoRequest.OldPassword is required. If
        //     the user forgot the old password, use the "/forgotPassword" endpoint instead.
        public string? NewPassword { get; init; }
        //
        // Summary:
        //     The old password for the authenticated user. This is only required if a Microsoft.AspNetCore.Identity.Data.InfoRequest.NewPassword
        //     is provided.
        public string? OldPassword { get; init; }

        [DefaultValue("https://localhost:7584/confirm-email")]
        [Required]
        public string ReturnUrl { get; init; }
    }
}
