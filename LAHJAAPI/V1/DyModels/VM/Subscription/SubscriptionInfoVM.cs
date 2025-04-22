using AutoGenerator;
using System.ComponentModel.DataAnnotations;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// Subscription  property for VM Info.
    /// </summary>
    public class SubscriptionInfoVM : ITVM
    {
        [Required]
        public string SubscriptionId { get; set; }
        [Required]
        public string ClientSecret { get; set; }
    }
}