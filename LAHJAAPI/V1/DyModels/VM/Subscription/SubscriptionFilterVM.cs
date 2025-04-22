using AutoGenerator;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// Subscription  property for VM Filter.
    /// </summary>
    public class SubscriptionFilterVM : ITVM
    {
        ///
        public string? Id { get; set; }
        ///
        public string? Lg { get; set; }

        public int AllowedSpaces { get; set; }
        public int SpaceCount { get; set; }
    }
}