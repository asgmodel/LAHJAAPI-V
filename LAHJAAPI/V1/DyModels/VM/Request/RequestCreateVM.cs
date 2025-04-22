using AutoGenerator;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// Request  property for VM Create.
    /// </summary>
    public class RequestCreateVM : ITVM
    {
        public String? Question { get; set; }

        public String? ServiceId { get; set; }
        ///
        public String? SpaceId { get; set; }

    }
}