using System.Net;

namespace Gaya_Server.Models
{
    public class Result<T>
    {
        public T? Value { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
