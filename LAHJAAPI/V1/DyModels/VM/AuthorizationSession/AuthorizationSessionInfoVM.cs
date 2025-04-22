using AutoGenerator;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// AuthorizationSession  property for VM Info.
    /// </summary>
    public class AuthorizationSessionInfoVM : ITVM
    {
        public string? SessionToken { get; set; }

        public string? URLCore { get; set; }
    }
}