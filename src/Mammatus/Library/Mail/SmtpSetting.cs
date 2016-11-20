namespace Mammatus.Library.Mail
{
    public class SmtpSetting
    {
        public string Server { get; set; }

        public bool Authentication { get; set; }

        public string User { get; set; }

        public string Sender { get; set; }

        public string Password { get; set; }
    }
}