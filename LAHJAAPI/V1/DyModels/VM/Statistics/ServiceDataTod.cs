namespace V1.DyModels.VM
{
    public class ServiceDataTod
    {
        public int Value { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
    }

    public class ModelAiServiceData
    {
        public string ModelAi { get; set; }
        public int UsageCount { get; set; }
    }

    public class SpaceRequestsAnalysis
    {
        public string Name { get; set; }
        public int UsageCount { get; set; }
    }
}
