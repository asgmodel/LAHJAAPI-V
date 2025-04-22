using System.ComponentModel.DataAnnotations;

namespace LAHJAAPI.Utilities
{
    public enum PauseCollectionBehaviorType
    {
        [Display(Name = "Keep As Draft")]
        keep_as_draft = 0, //  هذا يمنع إنشاء الفواتير أثناء فترة الإيقاف
        [Display(Name = "Mark Uncollectible")]
        mark_uncollectible = 1, // يحدد الفواتير على أنها غير قابلة للتحصيل
        [Display(Name = "Void")]
        @void = 2, // يلغي الفواتير عند الإيقاف 
    }
}
