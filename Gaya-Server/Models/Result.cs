using System.Net;

namespace Gaya_Server.Models
{
    /// <summary>
    /// The outcome of a service call, including the HTTP status the controller returns.
    /// </summary>
    public class Result<T>
    {
        public T? Value { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
