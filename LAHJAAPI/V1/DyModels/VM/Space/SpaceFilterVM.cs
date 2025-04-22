using AutoGenerator;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// Space  property for VM Filter.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Lg"></param>
    public record SpaceFilterVM(string? Id = null, string? Lg = null) : ITVM;
}