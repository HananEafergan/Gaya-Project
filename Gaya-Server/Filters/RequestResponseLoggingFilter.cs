using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gaya_Server.Filters
{
    public class RequestResponseLoggingFilter : IActionFilter
    {
        private readonly ILogger<RequestResponseLoggingFilter> _logger;

        public RequestResponseLoggingFilter(
            ILogger<RequestResponseLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation(
                "Request {Method} {Path} {@Arguments}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path,
                context.ActionArguments);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult result)
            {
                _logger.LogInformation(
                    "Response {StatusCode} {@Body}",
                    result.StatusCode,
                    result.Value);
            }
        }
    }
}