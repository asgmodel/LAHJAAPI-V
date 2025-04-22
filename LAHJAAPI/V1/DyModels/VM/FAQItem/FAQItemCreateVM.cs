using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using System;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// FAQItem  property for VM Create.
    /// </summary>
    public class FAQItemCreateVM : ITVM
    {
        //
        public TranslationData? Question { get; set; }
        //
        public TranslationData? Answer { get; set; }
        //
        public TranslationData? Tag { get; set; }
    }
}