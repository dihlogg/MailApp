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
        private const int Port = 11000;
        private UdpClient udpListener;
        private ObservableCollection<Email> emailList = new ObservableCollection<Email>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            EmailList.ItemsSource = emailList;
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

        //            if (message.StartsWith("REGISTER"))
        //            {
        //                string[] parts = message.Split(',');
        //                if (parts.Length > 1)
        //                {
        //                    string username = parts[1];

        //                    Dispatcher.Invoke(() => UpdateAccountList(username));

        //                    var welcomeEmail = new Email
        //                    {
        //                        Sender = "MailServer@gmail.com",
        //                        SenderName = "MailServer@gmail.com",
        //                        Recipient = username,
        //                        Subject = "Welcome!!!",
        //                        Body = "Your account has been created!!!. Thank you for using this service. We hope you will feel comfortable using our service",
        //                        ReceivedDate = DateTime.Now
        //                    };

        //                    // Cập nhật danh sách email
        //                    Dispatcher.Invoke(() => UpdateEmailList(welcomeEmail));
        //                }
        //            }
        //            else
        //            {
        //                string[] parts = message.Split('|');
        //                if (parts.Length == 5)
        //                {
        //                    string senderEmail = parts[0];
        //                    string recipientEmail = parts[1];
        //                    string subject = parts[2];
        //                    string body = parts[3];
        //                    string senderName = parts[4];

        //                    var email = new Email
        //                    {
        //                        Sender = senderEmail,
        //                        SenderName = senderName,
        //                        Recipient = recipientEmail,
        //                        Subject = subject,
        //                        Body = body,
        //                        ReceivedDate = DateTime.Now
        //                    };

        //                    Dispatcher.Invoke(() => UpdateEmailList(email));
        //                }
        //            }
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
                        HandleRegistration(message);
                    }
                    else
                    {
                        string[] parts = message.Split('|');
                        if (parts.Length == 5)
                        {
                            string senderEmail = parts[0];
                            string recipientEmail = parts[1];
                            string subject = parts[2];
                            string body = parts[3];
                            string senderName = parts[4];

                            // Check if the recipient is in the account list
                            if (AccountList.Items.Contains(recipientEmail))
                            {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error receiving data: {ex.Message}");
            }
        }

        private void HandleRegistration(string message)
        {
            string[] parts = message.Split(',');
            if (parts.Length > 1)
            {
                string username = parts[1];
                Dispatcher.Invoke(() => UpdateAccountList(username));

                var welcomeEmail = new Email
                {
                    Sender = "MailServer@gmail.com",
                    SenderName = "MailServer@gmail.com",
                    Recipient = username,
                    Subject = "Welcome!!!",
                    Body = "Your account has been created!!! Thank you for using this service. We hope you feel comfortable using it.",
                    ReceivedDate = DateTime.Now
                };

                Dispatcher.Invoke(() => UpdateEmailList(welcomeEmail));
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