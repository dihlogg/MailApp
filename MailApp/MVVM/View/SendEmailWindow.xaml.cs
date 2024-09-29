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
                string senderName = RegisterWindow.RegisteredUsername;
                string senderEmail = $"{senderName}";

                SendEmailViaUdp(senderName, senderEmail, recipient, subject, body);
                MessageBoxResult result = MessageBox.Show("Email sent successfully!", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);

                if (result == MessageBoxResult.OK)
                {
                    this.Close();
                }

                var newEmail = new Email
                {
                    Subject = subject,
                    Sender = senderName,
                    Recipient = recipient,
                    ReceivedDate = DateTime.Now,
                    Body = body
                };
                inboxEmailWindow.AddEmail(newEmail);

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while sending the mail: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SendEmailViaUdp(string senderName, string senderEmail, string recipient, string subject, string body)
        {
            using (UdpClient udpClient = new UdpClient())
            {
                string message = $"{senderEmail}|{recipient}|{subject}|{body}|{senderName}";
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                udpClient.Send(messageBytes, messageBytes.Length, "localhost", 11000);
            }
        }
    }
}
