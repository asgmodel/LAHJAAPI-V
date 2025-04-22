using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using System;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// ApplicationUser  property for VM Update.
    /// </summary>
    public class ApplicationUserUpdateVM : ITVM
    {
        ///
        public string? Id { get; set; }
        ///
        public ApplicationUserCreateVM? Body { get; set; }
    }
}