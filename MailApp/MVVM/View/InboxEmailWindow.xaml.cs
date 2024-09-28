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
            emails = new List<Email>();
            LoadEmails();
        }

        private void LoadEmails()
        {
            // Initialize with some default emails if needed
            emails.Add(new Email { Subject = "Welcome!", Sender = "server@example.com", ReceivedDate = DateTime.Now, Body = "Your account has been created." });
            InboxListBox.ItemsSource = emails;
        }

        public void AddEmail(Email newEmail)
        {
            emails.Add(newEmail);
            InboxListBox.Items.Refresh(); // Refresh the ListBox to show the new email
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
