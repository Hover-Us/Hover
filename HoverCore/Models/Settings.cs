namespace Hover.Models
{
    public class Settings
    {
        public Settings()
        {
            // Set default values.
            CaptchaSecret = "CaptchaSecret default in Settings.cs";
        }
        
        public string StaticSiteRoot { get; set; }
    //    public string CodeSiteRoot { get; set; }
        public string CaptchaSecret { get; set; }
        public string HashSalt { get; set; }
        public int IterationCount { get; set; }

        public string EmailClientHost { get; set; }
        public int EmailClientPort { get; set; }
        public bool EmailClientUseSsl { get; set; }
        public string EmailClientUserName { get; set; }
        public string EmailClientPassword { get; set; }

        public string MailboxAddressUpdateName { get; set; }
        public string MailboxAddressUsAddress { get; set; }
        public string MailboxAddressUpdateAddress { get; set; }
        public string MailboxAddressPrivacyAddress { get; set; }
        
    }
}
