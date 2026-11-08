namespace ModernRecrut.MVC.Models
{
    public class HttpCodeErrorViewModel
    {
        public int StatusCode { get; set; }
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string? ErrorMessage { get; set; }
    }
}
