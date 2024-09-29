using MailApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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
using System.Collections.ObjectModel;

namespace MailApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for InboxEmailWindow.xaml
    /// </summary>
    public partial class InboxEmailWindow : Window
    {
        private List<Email> emails;
        private string _username;
        private ObservableCollection<Email> emailList = new ObservableCollection<Email>();

        public InboxEmailWindow(string username)
        {
            InitializeComponent();
            _username = username;
            emails = new List<Email>();
            LoadEmails();

            HelloLabel.Content = "Hello: " + _username;
        }


        public void ReceiveEmail(string senderEmail, string recipient, string subject, string body)
        {
            if (_username == recipient)
            {
                var newEmail = new Email
                {
                    Subject = subject,
                    Sender = senderEmail,
                    Recipient = recipient,
                    ReceivedDate = DateTime.Now,
                    Body = body
                };

                emailList.Add(newEmail);
            }
        }

        private void LoadEmails()
        {
            emails.Add(new Email { Subject = "Welcome!!!", Sender = "MailServer@gmail.com", ReceivedDate = DateTime.Now, Body = "Your account has been created!!!. Thank you for using this service. We hope you will feel comfortable using our service" });
            InboxListBox.ItemsSource = emails;
        }

        public void AddEmail(Email newEmail)
        {
            emails.Add(newEmail);
            InboxListBox.Items.Refresh();
        }

        private void InboxListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InboxListBox.SelectedItem is Email selectedEmail)
            {
                EmailSubjectTextBlock.Text = selectedEmail.Subject;
                EmailSenderTextBlock.Text = selectedEmail.Sender;
                EmailDateTextBlock.Text = selectedEmail.ReceivedDate.ToString("g");
                EmailBodyTextBox.Text = selectedEmail.Body;

                EmailDetailsPanel.Visibility = Visibility.Visible;
            }
            else
            {
                EmailDetailsPanel.Visibility = Visibility.Collapsed;
                EmailSubjectTextBlock.Text = string.Empty;
                EmailSenderTextBlock.Text = string.Empty;
                EmailDateTextBlock.Text = string.Empty;
                EmailBodyTextBox.Text = string.Empty;
            }
        }
        private void OpenSendEmailWindow(object sender, RoutedEventArgs e)
        {
            SendEmailWindow sendEmailWindow = new SendEmailWindow(this);
            sendEmailWindow.Show();
        }
    }
}
