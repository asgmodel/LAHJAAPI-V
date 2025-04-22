using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using System;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// PlanFeature  property for VM Create.
    /// </summary>
    public class PlanFeatureCreateVM : ITVM
    {
        //
        public TranslationData? Name { get; set; }
        //
        public TranslationData? Description { get; set; }
        ///
        public String? PlanId { get; set; }
        public PlanCreateVM? Plan { get; set; }
    }
}