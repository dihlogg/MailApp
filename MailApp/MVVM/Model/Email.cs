using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailApp.MVVM.Model
{
    public class Email
    {
        public string Subject { get; set; }
        public string Sender { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string Body { get; set; }
    }
}
