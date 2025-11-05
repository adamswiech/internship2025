namespace MVC_TEST.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        
    }
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
