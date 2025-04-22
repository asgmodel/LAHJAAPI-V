using AutoGenerator;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// EventRequest  property for VM Output.
    /// </summary>
    public class EventRequestOutputVM : ITVM
    {
        ///
        public String? Id { get; set; }
        ///
        public String? Status { get; set; }
        ///
        public String? Details { get; set; }
        ///
        public DateTime CreatedAt { get; set; }
        ///
        public String? RequestId { get; set; }
    }
}