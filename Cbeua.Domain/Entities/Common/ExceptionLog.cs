namespace Cbeua.Domain.Entities
{
    public class ExceptionLog
    {
        public int Id { get; set; }
        public DateTime LoggedAt { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public string QueryString { get; set; }
        public string User { get; set; }
        public string Headers { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string InnerException { get; set; } = "";
        public string TraceId { get; set; }
    }
}
