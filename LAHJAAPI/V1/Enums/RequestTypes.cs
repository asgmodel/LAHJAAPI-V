using System.ComponentModel.DataAnnotations;

namespace LAHJAAPI.V1.Enums
{
    public enum RequestStatus
    {
        [Display(Name = "111.1")]
        Created = 1111,
        [Display(Name = "222.1")]
        Processing = 2221,
        [Display(Name = "222.2")]
        Success = 2222,
        [Display(Name = "333.1")]
        Failed = 3331,//error api model
        [Display(Name = "333.2")]
        FailedApiCore = 3332, //error api core
        [Display(Name = "444.1")]
        Retry = 4441
    }

    public enum EventRequestStatus
    {
        [Display(Name = "222.2")]
        Success = 2222,
        [Display(Name = "333.1")]
        Failed = 3331,//error api model

    }
    public enum RequestType
    {
        All,
        Requests,
        Errors,
    }

    public enum FilterBy
    {
        Service,
        Space,
        Model,
    }
}
