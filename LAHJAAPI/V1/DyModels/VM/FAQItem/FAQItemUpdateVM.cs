using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using System;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// FAQItem  property for VM Update.
    /// </summary>
    public class FAQItemUpdateVM : ITVM
    {
        ///
        public string? Id { get; set; }
        ///
        public FAQItemCreateVM? Body { get; set; }
    }
}