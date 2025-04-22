using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using System;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// Dialect  property for VM Update.
    /// </summary>
    public class DialectUpdateVM : ITVM
    {
        ///
        public string? Id { get; set; }
        ///
        public DialectCreateVM? Body { get; set; }
    }
}