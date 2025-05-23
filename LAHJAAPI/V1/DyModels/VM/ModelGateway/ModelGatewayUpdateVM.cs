using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using System;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// ModelGateway  property for VM Update.
    /// </summary>
    public class ModelGatewayUpdateVM : ITVM
    {
        ///
        public string? Id { get; set; }
        ///
        public ModelGatewayCreateVM? Body { get; set; }
    }
}