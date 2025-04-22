using Microsoft.AspNetCore.Mvc;

namespace LAHJAAPI.Utilities;

public class HandelErrors
{
    private readonly object _value;

    public bool IsSuccess { get; }
    public bool IsFailuer => !IsSuccess;

    public object Data
    {
        get
        {
            //if (!IsSuccess) throw new InvalidOperationException("there is no value for failure");
            return _value;
        }
        private init => _value = value;
    }


    public HandelErrors(object value)
    {
        Data = value;
        IsSuccess = true;
        //Error = Error.None;
    }

    public HandelErrors(bool IsSuccess = true) { this.IsSuccess = IsSuccess; }



    public static HandelErrors Ok()
    {
        return new HandelErrors();
    }

    public static HandelErrors Ok(object value)
    {
        var r = new HandelErrors(value);
        return r;
    }

    public static ProblemDetails Problem(Exception ex)
    {
        return new ProblemDetails
        {
            Type = ex.GetType().FullName,
            Title = ex.Message,
            Detail = ex.InnerException?.Message,

            //Status=ex.cod
        };
    }

    public static ProblemDetails Problem(string title, string details, string? type = null, int? status = null)
    {
        return new ProblemDetails
        {
            Title = title,
            Detail = details,
            Type = type,
            Status = status
        };
    }

    public static ProblemDetails NotFound(string details, string? type = null, string title = "Not Found", int status = StatusCodes.Status404NotFound)
    {
        return new ProblemDetails
        {
            Title = title,
            Detail = details,
            Type = type,
            Status = status
        };
    }
}
