using AutoGenerator;
using AutoGenerator.Helper.Translation;
using LAHJAAPI.Models;
using System;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// Service  property for VM Output.
    /// </summary>
    public class ServiceOutputVM : ITVM
    {
        ///
        public String? Id { get; set; }
        ///
        public String? Name { get; set; }
        ///
        public String? AbsolutePath { get; set; }
        ///
        public String? Token { get; set; }
        ///
        public String? ModelAiId { get; set; }
        public ModelAiOutputVM? ModelAi { get; set; }
        //
        public List<ServiceMethodOutputVM>? ServiceMethods { get; set; }
        //
        public List<UserServiceOutputVM>? UserServices { get; set; }
        //
        public List<RequestOutputVM>? Requests { get; set; }
    }
}