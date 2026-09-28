using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gaya_Server.Filters
{
    /// <summary>
    /// Logs each controller request before the action runs and the response body after it returns.
    /// </summary>
    public class RequestResponseLoggingFilter : IActionFilter
    {
        private readonly ILogger<RequestResponseLoggingFilter> _logger;

        public RequestResponseLoggingFilter(
            ILogger<RequestResponseLoggingFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Logs the HTTP method, path, and the arguments bound for the action.
        /// </summary>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation(
                "Request {Method} {Path} {@Arguments}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path,
                context.ActionArguments);
        }

        /// <summary>
        /// Logs the status code and body when the action returned an object result.
        /// </summary>
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