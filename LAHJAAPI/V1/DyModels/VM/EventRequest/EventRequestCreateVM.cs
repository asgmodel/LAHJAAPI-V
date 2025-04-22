using AutoGenerator;
using LAHJAAPI.V1.Enums;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// EventRequest  property for VM Create.
    /// </summary>
    public class EventRequestCreateVM : ITVM
    {
        public required String EventId { get; set; }
        public required EventRequestStatus Status { get; set; }
        public required String Details { get; set; }
    }
}