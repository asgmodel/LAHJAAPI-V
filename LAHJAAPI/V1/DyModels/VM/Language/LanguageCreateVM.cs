using AutoGenerator;
using AutoGenerator.Helper.Translation;

namespace V1.DyModels.VMs
{
    /// <summary>
    /// Language  property for VM Create.
    /// </summary>
    public class LanguageCreateVM : ITVM
    {
        ///
        public TranslationData? Name { get; set; }
        ///
        public String? Code { get; set; }
    }
}