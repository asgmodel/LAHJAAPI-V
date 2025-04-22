using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using System;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// Plan  property for VM Update.
    /// </summary>
    public class PlanUpdateVM : ITVM
    {
        ///
        public string? Id { get; set; }
        ///
        public PlanCreateVM? Body { get; set; }
    }
}