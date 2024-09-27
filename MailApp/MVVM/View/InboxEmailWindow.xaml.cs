using MailApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for InboxEmailWindow.xaml
    /// </summary>
    public partial class InboxEmailWindow : Window
    {
        private List<Email> emails;
        public InboxEmailWindow()
        {
            InitializeComponent();
            LoadEmails();
        }
        private void LoadEmails()
        {
            List<Email> emails = new List<Email>
            {
                new Email { Subject = "Email 1", Sender = "sender1@example.com", ReceivedDate = DateTime.Now, Body = "This is the body of email 1." },
                new Email { Subject = "Email 2", Sender = "sender2@example.com", ReceivedDate = DateTime.Now, Body = "This is the body of email 2." },
                // Thêm các email khác ở đây
            };

            InboxListBox.ItemsSource = emails;
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
            SendEmailWindow sendEmailWindow = new SendEmailWindow();
            sendEmailWindow.Show();
        }
    }
}
