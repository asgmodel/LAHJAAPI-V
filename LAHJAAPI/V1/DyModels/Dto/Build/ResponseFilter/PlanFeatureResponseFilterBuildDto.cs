using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using AutoGenerator.Config;
using System;

namespace V1.DyModels.Dto.Build.ResponseFilters
{
    public class PlanFeatureResponseFilterBuildDto : ITBuildDto
    {
        /// <summary>
        /// Id property for DTO.
        /// </summary>
        public Int32 Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        /// <summary>
        /// PlanId property for DTO.
        /// </summary>
        public String? PlanId { get; set; }
        public PlanResponseFilterBuildDto? Plan { get; set; }

        [FilterLGEnabled]
        public string? Lg { get; set; }
    }
}