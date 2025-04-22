using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using System;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// Request  property for VM Update.
    /// </summary>
    public class RequestUpdateVM : ITVM
    {
        ///
        public string? Id { get; set; }
        ///
        public RequestCreateVM? Body { get; set; }
    }
}