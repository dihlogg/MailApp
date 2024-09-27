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

namespace MailServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int Port = 11000;
        private UdpClient udpListener;

        public MainWindow()
        {
            InitializeComponent();
            StartUdpListener();
        }

        private void StartUdpListener()
        {
            udpListener = new UdpClient(Port);
            Thread listenerThread = new Thread(new ThreadStart(ListenForMessages));
            listenerThread.IsBackground = true;
            listenerThread.Start();
        }

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
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error receiving data: {ex.Message}");
            }
        }

        private void UpdateAccountList(string username)
        {
            if (!AccountList.Items.Contains(username))
            {
                AccountList.Items.Add(username);
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

        private void EmailList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Handle email selection changes if needed
        }
    }
}
