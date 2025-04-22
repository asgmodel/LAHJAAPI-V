using LAHJAAPI.V1.Enums;
using System.ComponentModel;

namespace V1.DyModels.VM
{
    public class RequestData
    {
        public object? DateTime { get; set; }
        public int Requests { get; set; }
        public int Errors { get; set; }
        public string? Name { get; set; }

    }

    public class RequestErrorsResponse
    {
        public string RequestId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ServiceType { get; set; }
        public int Errors { get; set; }
        //public ICollection<EventRequestResponse> Events { get; set; }

    }


    public class FilterServiceReques
    {

        [DefaultValue("All")]
        public RequestType RequestType { get; set; } = RequestType.All;
        //[DefaultValue(DateTime.UtcNow.ToString())]
        [DefaultValue(null)]
        public DateTime? StartDate { get; set; }
        [DefaultValue(null)]
        public DateTime? EndDate { get; set; } = null;

        public DateTimeFilter GroupBy { get; set; } = DateTimeFilter.Day;
    }
}
