using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using System;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// Subscription  property for VM Update.
    /// </summary>
    public class SubscriptionUpdateVM : ITVM
    {
        ///
        public string? Id { get; set; }
        ///
        public SubscriptionCreateVM? Body { get; set; }
    }
}