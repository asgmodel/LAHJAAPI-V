using AutoGenerator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LAHJAAPI.Utilities;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// Subscription  property for VM Create.
    /// </summary>
    public class SubscriptionCreateVM : ITVM
    {
        [Required]
        public String PlanId { get; set; }

    }

    public class SubscriptionResumeRequest
    {
        [DefaultValue("create_prorations")]
        public string ProrationBehavior { get; set; } = "create_prorations"; // احتساب الفروقات
    }


    public class SubscriptionUpdateRequest
    {
        [DefaultValue("void")]
        public PauseCollectionBehaviorType PauseCollectionBehavior { get; set; } = PauseCollectionBehaviorType.@void; // سلوك الفواتير أثناء الإيقاف
        public DateTime? ResumesAt { get; set; }
    }

}