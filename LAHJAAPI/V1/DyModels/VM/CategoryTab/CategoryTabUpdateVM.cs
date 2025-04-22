using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using System;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// CategoryTab  property for VM Update.
    /// </summary>
    public class CategoryTabUpdateVM : ITVM
    {
        ///
        public string? Id { get; set; }
        ///
        public CategoryTabCreateVM? Body { get; set; }
    }
}