using AutoGenerator;

namespace V1.DyModels.Dto.Build.Responses
{
    public class ModelAiResponseBuildDto : ITBuildDto
    {
        /// <summary>
        /// Id property for DTO.
        /// </summary>
        public String? Id { get; set; }
        public String? Name { get; set; }
        /// <summary>
        /// Token property for DTO.
        /// </summary>
        public String? Token { get; set; }
        /// <summary>
        /// AbsolutePath property for DTO.
        /// </summary>
        public String? AbsolutePath { get; set; }
        public String? Category { get; set; }
        public String? Language { get; set; }
        public bool IsStandard { get; set; }
        public String? Gender { get; set; }
        public String? Dialect { get; set; }
        /// <summary>
        /// Type property for DTO.
        /// </summary>
        public String? Type { get; set; }
        /// <summary>
        /// ModelGatewayId property for DTO.
        /// </summary>
        public String? ModelGatewayId { get; set; }

        public ModelGatewayResponseBuildDto? ModelGateway { get; set; }
        public ICollection<ServiceResponseBuildDto>? Services { get; set; }

        public ICollection<UserModelAiResponseBuildDto>? UserModelAis { get; set; }
    }
}