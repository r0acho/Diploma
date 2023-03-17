using System.Net;

namespace Diploma.Domain.Response;

public record BaseResponse
{
    public string? Description { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}