using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using System;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// Service  property for VM Filter.
    /// </summary>
    public class ServiceFilterVM : ITVM
    {
        ///
        public string? Id { get; set; }
        ///
        public string? Lg { get; set; }
    }
}