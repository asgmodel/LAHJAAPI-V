using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace V1.DyModels.VMs
{
    public class EncryptTokenRequest
    {
        [DefaultValue("internal")]
        [Required]
        public string AuthorizationType { get; set; } = "internal";
        [DefaultValue(null)]
        public string? SpaceId { get; set; } = string.Empty;
        //[DefaultValue(null)]
        public DateTime? Expires { get; set; }
    }

    public class DataTokenRequest
    {
        public string AuthorizationType { get; set; }

        public string? SpaceId { get; set; }
        public DateTime? Expires { get; set; }
        public string Token { get; set; }
    }
}
