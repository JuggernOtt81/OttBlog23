namespace OttBlog23.ViewModels
{
    public class MailSettings
    {
        //to configure and use an smtp server (google)
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
        public string? MailPassword { get; set; }
        public string? MailHost { get; set; }
        public int MailPort { get; set; }
    }
}
