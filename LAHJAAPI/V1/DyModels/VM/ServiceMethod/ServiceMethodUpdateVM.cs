using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using System;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// ServiceMethod  property for VM Update.
    /// </summary>
    public class ServiceMethodUpdateVM : ITVM
    {
        ///
        public string? Id { get; set; }
        ///
        public ServiceMethodCreateVM? Body { get; set; }
    }
}