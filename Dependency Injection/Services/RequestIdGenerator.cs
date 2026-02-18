using System;

namespace DependencyInjection.Services
{
    /*
     * A class that generates a unique request ID for each instance.
     */
    public class RequestIdGenerator : IRequestIdGenerator
    {
        // Constructor creates a new unique ID when the object is made
        public RequestIdGenerator()
        {
            this.RequestId = Guid.NewGuid().ToString();
        }

        // The unique request ID (read-only)
        public string RequestId { get; }
    }
}
