using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using AutoGenerator.Config;
using System;

namespace V1.DyModels.Dto.Build.ResponseFilters
{
    public class ModelGatewayResponseFilterBuildDto : ITBuildDto
    {
        /// <summary>
        /// Id property for DTO.
        /// </summary>
        public String? Id { get; set; }
        /// <summary>
        /// Name property for DTO.
        /// </summary>
        public String? Name { get; set; }
        /// <summary>
        /// Url property for DTO.
        /// </summary>
        public String? Url { get; set; }
        /// <summary>
        /// Token property for DTO.
        /// </summary>
        public String? Token { get; set; }
        /// <summary>
        /// IsDefault property for DTO.
        /// </summary>
        public Boolean IsDefault { get; set; }

        [FilterLGEnabled]
        public string? Lg { get; set; }
    }
}