using AutoGenerator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// AuthorizationSession  property for VM Create.
    /// </summary>
    public class AuthorizationSessionCreateVM : ITVM
    {
        public required string Token { get; set; }

        public required string ServiceId { get; set; }
    }

    public class CreateAuthorizationWebRequest
    {

        public required string Token { get; set; }

        public required string ServiceId { get; set; }

    }

    public class CreateAuthorizationForDashboard
    {
        public required string Token { get; set; }
    }

    public class CreateAuthorizationForListServices
    {

        public required string Token { get; set; }

        [Required(ErrorMessage = "The ServicesIds field is required.")]
        public List<string> ServicesIds { get; set; }

    }

    public class CreateAuthorizationForServices
    {

        public required string Token { get; set; }

        [DefaultValue("")]
        public string? ModelAiId { get; set; }

    }
}