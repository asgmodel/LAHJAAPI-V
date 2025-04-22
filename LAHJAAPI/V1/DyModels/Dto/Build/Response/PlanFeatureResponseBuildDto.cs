using AutoGenerator;
using AutoGenerator.Helper.Translation;

namespace V1.DyModels.Dto.Build.Responses
{
    public class PlanFeatureResponseBuildDto : ITBuildDto
    {
        /// <summary>
        /// Id property for DTO.
        /// </summary>
        public Int32 Id { get; set; }

        public string? Key { get; set; }
        public string? Value { get; set; }

        public TranslationData? Name { get; set; } = new();
        public TranslationData? Description { get; set; } = new();
        /// <summary>
        /// PlanId property for DTO.
        /// </summary>
        public String? PlanId { get; set; }
        public PlanResponseBuildDto? Plan { get; set; }
    }
}