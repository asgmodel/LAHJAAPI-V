using AutoGenerator;
using AutoGenerator.Helper.Translation;

namespace V1.DyModels.Dto.Build.Requests
{
    public class TypeModelRequestBuildDto : ITBuildDto
    {
        /// <summary>
        /// Id property for DTO.
        /// </summary>
        public string Id { get; set; } = $"type_{Guid.NewGuid():N}";
        public TranslationData? Name { get; set; } = new();
        public TranslationData? Description { get; set; } = new();
        /// <summary>
        /// Active property for DTO.
        /// </summary>
        public Boolean Active { get; set; }
        /// <summary>
        /// Image property for DTO.
        /// </summary>
        public String? Image { get; set; }
    }
}