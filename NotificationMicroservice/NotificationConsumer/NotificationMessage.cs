namespace NotificationMicroservice
{
    public class NotificationMessage
    {
        public string To { get; set; }
        public string Body { get; set; }
        public string Channel { get; set; }
        public int TemplateID { get; set; }
        public Dictionary<string, string> Parameters { get;set; }
    }
}