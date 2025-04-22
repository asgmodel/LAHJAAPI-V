using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using System;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// Setting  property for VM Create.
    /// </summary>
    public class SettingCreateVM : ITVM
    {
        ///
        public String? Name { get; set; }
        ///
        public String? Value { get; set; }
    }
}