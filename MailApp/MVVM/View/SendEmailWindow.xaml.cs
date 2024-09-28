using MailApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MailApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for SendEmailWindow.xaml
    /// </summary>
    public partial class SendEmailWindow : Window
    {
        private InboxEmailWindow inboxEmailWindow;

        public SendEmailWindow(InboxEmailWindow inbox)
        {
            InitializeComponent();
            inboxEmailWindow = inbox;
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string recipient = RecipientTextBox.Text;
            string subject = SubjectTextBox.Text;
            string body = BodyTextBox.Text;

            try
            {
                string senderName = RegisterWindow.RegisteredUsername; // Get the registered username

                SendEmailViaUdp(senderName, "test1@localhost.com", recipient, subject, body); // Pass the senderName
                MessageBox.Show("Email đã được gửi thành công qua UDP!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                // Create a new Email object to add to the inbox
                var newEmail = new Email
                {
                    Subject = subject,
                    Sender = senderName, // Use the senderName here
                    ReceivedDate = DateTime.Now, // You can adjust this as necessary
                    Body = body
                };
                inboxEmailWindow.AddEmail(newEmail); // Add email to the inbox
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi gửi email: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SendEmailViaUdp(string senderName, string senderEmail, string recipient, string subject, string body)
        {
            using (UdpClient udpClient = new UdpClient())
            {
                string message = $"{senderEmail}|{recipient}|{subject}|{body}|{senderName}"; // Add sender name
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                udpClient.Send(messageBytes, messageBytes.Length, "localhost", 11000); // Send to UDP server
            }
        }
    }

    //private void SendEmail(string recipient, string subject, string body)
    //    {
    //        //SmtpClient smtpClient = new SmtpClient("smtp.your-email-server.com")
    //        //{
    //        //    Port = 587,
    //        //    Credentials = new System.Net.NetworkCredential("your-email@example.com", "your-password"),
    //        //    EnableSsl = true,

    //        //};

    //        //// Tạo đối tượng MailMessage để gửi email
    //        //MailMessage mail = new MailMessage
    //        //{
    //        //    From = new MailAddress("your-email@example.com"),
    //        //    Subject = subject,
    //        //    Body = body,
    //        //    IsBodyHtml = false
    //        //};

    //        //mail.To.Add(recipient);
    //        //smtpClient.Send(mail);
    //        MailMessage mail = new MailMessage();
    //        SmtpClient smtpServer = new SmtpClient("localhost");

    //        mail.From = new MailAddress("test1@localhost.com"); // Your valid email
    //        mail.To.Add("test2@localhost.com");                  // Recipient's valid email
    //        mail.Subject = "Test Mail";
    //        mail.Body = "This is for testing SMTP mail from C#";

    //        smtpServer.Port = 2525;
    //        //smtpServer.Credentials = new NetworkCredential("your-email@example.com", "your-password");
    //        smtpServer.EnableSsl = false;

    //        smtpServer.Send(mail);
    //    }
    //}
}
