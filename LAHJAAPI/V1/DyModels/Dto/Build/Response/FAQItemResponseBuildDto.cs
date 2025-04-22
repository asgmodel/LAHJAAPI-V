using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using AutoGenerator.Config;
using System;

namespace V1.DyModels.Dto.Build.Responses
{
    public class FAQItemResponseBuildDto : ITBuildDto
    {
        /// <summary>
        /// Id property for DTO.
        /// </summary>
        public String? Id { get; set; }
        public TranslationData? Question { get; set; } = new();
        public TranslationData? Answer { get; set; } = new();
        public TranslationData? Tag { get; set; } = new();
    }
}