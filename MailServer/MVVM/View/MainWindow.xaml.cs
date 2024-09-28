using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Net.Sockets;
using System.Net;
using MailServer.Net;
using System.Collections.ObjectModel;
using System.Net.Mail;
using MailServer.MVVM.Model;

namespace MailServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> Emails { get; set; } = new ObservableCollection<string>();
        private const int Port = 11000; // UDP listening port
        private UdpClient udpListener;
        private ObservableCollection<Email> emailList = new ObservableCollection<Email>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this; // Set the DataContext to the current instance
            EmailList.ItemsSource = emailList; // Set the initial ItemsSource
            StartUdpListener();
        }

        private void StartUdpListener()
        {
            udpListener = new UdpClient(Port);
            Thread listenerThread = new Thread(new ThreadStart(ListenForMessages));
            listenerThread.IsBackground = true;
            listenerThread.Start();
        }

        //private void ListenForMessages()
        //{
        //    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, Port);

        //    try
        //    {
        //        while (true)
        //        {
        //            byte[] receivedData = udpListener.Receive(ref remoteEndPoint);
        //            string message = Encoding.UTF8.GetString(receivedData);

        //            Dispatcher.Invoke(() =>
        //            {
        //                if (!string.IsNullOrEmpty(message))
        //                {
        //                    // Parse the received message
        //                    string[] parts = message.Split('|');
        //                    if (parts.Length == 4)
        //                    {
        //                        string senderEmail = parts[0];
        //                        string recipientEmail = parts[1];
        //                        string subject = parts[2];
        //                        string body = parts[3];

        //                        // Send the email via SMTP
        //                        bool emailSent = SendEmail(senderEmail, recipientEmail, subject, body);
        //                        Emails.Add(emailSent ? $"Email sent from {senderEmail} to {recipientEmail}" : $"Failed to send email from {senderEmail}");
        //                    }
        //                    else
        //                    {
        //                        Emails.Add("Invalid email message format.");
        //                    }
        //                }
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error receiving data: {ex.Message}");
        //    }
        //}

        private void ListenForMessages()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, Port);

            try
            {
                while (true)
                {
                    byte[] receivedData = udpListener.Receive(ref remoteEndPoint);
                    string message = Encoding.UTF8.GetString(receivedData);

                    if (message.StartsWith("REGISTER"))
                    {
                        string[] parts = message.Split(',');
                        if (parts.Length > 1)
                        {
                            string username = parts[1];
                            Dispatcher.Invoke(() => UpdateAccountList(username));
                        }
                    } else
                    {
                        string[] parts = message.Split('|');
                        if (parts.Length == 5)
                        {
                            string senderEmail = parts[0];
                            string recipientEmail = parts[1];
                            string subject = parts[2];
                            string body = parts[3];
                            string senderName = parts[4]; // Get the sender's name

                            // Create a new email object
                            var email = new Email
                            {
                                Sender = senderEmail,
                                SenderName = senderName,
                                Recipient = recipientEmail,
                                Subject = subject,
                                Body = body,
                                ReceivedDate = DateTime.Now
                            };
                            Dispatcher.Invoke(() => UpdateEmailList(email));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error receiving data: {ex.Message}");
            }
        }

        private void UpdateEmailList(Email email)
        {
            emailList.Add(email);

            EmailList.ItemsSource = emailList;
        }

        private void UpdateAccountList(string username)
        {
            if (!AccountList.Items.Contains(username))
            {
                AccountList.Items.Add(username);
            }
        }

        private bool SendEmail(string from, string to, string subject, string body)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("localhost", 2525)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = false,
                };

                MailMessage mailMessage = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false,
                };

                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending email: {ex.Message}");
                return false;
            }
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            // Optionally refresh the list or perform other actions
        }

        private void AccountList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Handle account selection changes if needed
        }

        private void EmailList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmailList.SelectedItem is Email selectedEmail)
            {
                EmailSubjectTextBlock.Text = selectedEmail.Subject;
                EmailSenderTextBlock.Text = selectedEmail.SenderName;
                EmaiRecipientTextBlock.Text = selectedEmail.Recipient;
                EmailDateTextBlock.Text = selectedEmail.ReceivedDate.ToString("g");
                EmailBodyTextBox.Text = selectedEmail.Body;

                EmailDetailsPanel.Visibility = Visibility.Visible;
            }
            else
            {
                EmailDetailsPanel.Visibility = Visibility.Collapsed;
                EmailSubjectTextBlock.Text = string.Empty;
                EmailSenderTextBlock.Text = string.Empty;
                EmaiRecipientTextBlock.Text = string.Empty;
                EmailDateTextBlock.Text = string.Empty;
                EmailBodyTextBox.Text = string.Empty;
            }
        }
    }
}
