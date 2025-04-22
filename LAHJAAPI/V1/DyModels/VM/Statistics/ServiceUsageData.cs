namespace V1.DyModels.VM
{
    public class UsedRequestsVm
    {
        public string? Name { get; set; }
        public int UsageCount { get; set; }
        public int? Remaining { get; set; }
    }
    public class ServiceUsersCount
    {
        public string ServiceType { get; set; }
        public int Count { get; set; }
    }
}
