using System;

namespace Cbeua.Core.Helpers
{
    /// <summary>
    /// Thrown when a request breaks a business rule (e.g. editing an approved entry).
    /// ExceptionLoggingMiddleware returns its message to the caller with HTTP 400.
    /// </summary>
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }
    }
}
