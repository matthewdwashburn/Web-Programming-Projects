using DependencyInjection.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DependencyInjection.Filters
{
    // Adds a unique request ID to each HTTP response
    public class RequestIdFilter : IActionFilter
    {
        // Generator for unique request IDs
        private readonly IRequestIdGenerator requestIdGenerator;

        // Constructor injects the request ID generator
        public RequestIdFilter(RequestIdGenerator requestIdGenerator)
        {
            this.requestIdGenerator = requestIdGenerator;
        }

        // Called after the action executes
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Log and add the request ID to the response headers
            ConsoleLogger.Instance.Log("Adding a request-id to the response: " + requestIdGenerator.RequestId);
            context.HttpContext.Response.Headers["request-id"] = requestIdGenerator.RequestId;
        }

        // Called before the action executes
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Nothing needed here, but required by the interface
        }
    }
}
