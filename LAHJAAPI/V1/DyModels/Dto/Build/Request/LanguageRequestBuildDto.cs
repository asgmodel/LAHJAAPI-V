using AutoGenerator;

namespace V1.DyModels.Dto.Build.Requests
{
    public class LanguageRequestBuildDto : ITBuildDto
    {
        /// <summary>
        /// Id property for DTO.
        /// </summary>
        public string Id { get; set; } = $"lang_{Guid.NewGuid():N}";
        /// <summary>
        /// Name property for DTO.
        /// </summary>
        public String? Name { get; set; }
        /// <summary>
        /// Code property for DTO.
        /// </summary>
        public String? Code { get; set; }
    }
}