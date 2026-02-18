namespace DependencyInjection.Services
{
    // Defines a contract for generating a unique request ID
    public interface IRequestIdGenerator
    {
        // A string that holds the unique request ID
        public string RequestId { get; }
    }
}
