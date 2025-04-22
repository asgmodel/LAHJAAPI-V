using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using System;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// Advertisement  property for VM Update.
    /// </summary>
    public class AdvertisementUpdateVM : ITVM
    {
        ///
        public string? Id { get; set; }
        ///
        public AdvertisementCreateVM? Body { get; set; }
    }
}