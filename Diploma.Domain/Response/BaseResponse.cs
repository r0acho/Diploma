using System.Net;

namespace Diploma.Domain.Response;


public class BaseResponse
{
    public string? Description { get; set; }
    public HttpStatusCode StatusCode { get; set; }

}