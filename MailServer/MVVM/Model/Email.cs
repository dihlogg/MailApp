using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailServer.MVVM.Model
{
    public class Email
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime SentDate { get; set; } = DateTime.Now;

        public Email(string sender, string recipient, string subject, string body)
        {
            Sender = sender;
            Recipient = recipient;
            Subject = subject;
            Body = body;
        }
    }
}
