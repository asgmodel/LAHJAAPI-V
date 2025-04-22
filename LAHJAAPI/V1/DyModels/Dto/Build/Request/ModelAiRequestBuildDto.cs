using AutoGenerator;
using AutoGenerator.Helper.Translation;

namespace V1.DyModels.Dto.Build.Requests
{
    public class ModelAiRequestBuildDto : ITBuildDto
    {
        /// <summary>
        /// Id property for DTO.
        /// </summary>
       // public String? Id { get; set; }
        public TranslationData? Name { get; set; } = new();
        /// <summary>
        /// Token property for DTO.
        /// </summary>
        public String? Token { get; set; }
        /// <summary>
        /// AbsolutePath property for DTO.
        /// </summary>
        public String? AbsolutePath { get; set; }
        public TranslationData? Category { get; set; } = new();
        public string? Language { get; set; }
        public bool? IsStandard { get; set; }
        public string? Gender { get; set; }
        public TranslationData? Dialect { get; set; } = new();
        /// <summary>
        /// Type property for DTO.
        /// </summary>
        public String? Type { get; set; }
        /// <summary>
        /// ModelGatewayId property for DTO.
        /// </summary>
        public String? ModelGatewayId { get; set; }

    }
}