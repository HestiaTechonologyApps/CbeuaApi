namespace Cbeua.Domain.DTO
{
    public class CustomApiResponse
    {
        public int StatusCode { get; set; }
        public string Error { get; set; }
        public string CustomMessage { get; set; }
        public bool IsSucess { get; set; }
        public object Value { get; set; }
    }
   
}
