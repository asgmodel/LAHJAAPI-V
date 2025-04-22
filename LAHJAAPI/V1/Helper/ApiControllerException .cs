using Microsoft.AspNetCore.Mvc;

namespace LAHJAAPI.V1.Helper
{
   public class ApiControllerException : Exception
{
    public string Title { get; set; }
    public string Detail { get; set; }
    public int Status { get; set; }
    public string Type { get; set; }

    public ProblemDetails ProblemDetails { get; set; }

    public ApiControllerException(ProblemDetails problem, string detail = "")
        : base(detail)
    {
        ProblemDetails = problem;
        Title = problem.Title;
        Detail = problem.Detail ?? detail;
        Status = problem.Status ?? 500;
        Type = problem.Type;
    }
}
}
 
