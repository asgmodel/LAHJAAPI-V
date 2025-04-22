using AutoGenerator;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// PlanFeature  property for VM Output.
    /// </summary>
    public class PlanFeatureOutputVM : ITVM
    {
        ///
        public Int32 Id { get; set; }
        //
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? Name { get; set; }
        //
        public string? Description { get; set; }
        ///
        public String? PlanId { get; set; }
        public PlanOutputVM? Plan { get; set; }
    }
}