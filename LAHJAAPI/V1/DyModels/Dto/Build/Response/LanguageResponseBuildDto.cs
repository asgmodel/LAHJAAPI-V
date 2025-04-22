using AutoGenerator;
using AutoGenerator.Helper.Translation;

namespace V1.DyModels.Dto.Build.Responses
{
    public class LanguageResponseBuildDto : ITBuildDto
    {
        /// <summary>
        /// Id property for DTO.
        /// </summary>
        public String? Id { get; set; }
        /// <summary>
        /// Name property for DTO.
        /// </summary>
        public TranslationData? Name { get; set; } = new();
        /// <summary>
        /// Code property for DTO.
        /// </summary>
        public String? Code { get; set; }
    }
}