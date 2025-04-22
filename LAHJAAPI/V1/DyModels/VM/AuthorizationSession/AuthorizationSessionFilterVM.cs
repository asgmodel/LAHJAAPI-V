using AutoGenerator;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// AuthorizationSession  property for VM Filter.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Lg"></param>
    public record AuthorizationSessionFilterVM(string? Id, string? Lg) : ITVM;
}