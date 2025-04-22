using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using System;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// Request  property for VM Delete.
    /// </summary>
    public class RequestDeleteVM : ITVM
    {
        ///
        public string? Id { get; set; }
    }
}