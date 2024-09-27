using MailApp.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private const int Port = 11000;
        private const string ServerIp = "127.0.0.1"; // Change to MailServer's IP if needed

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;

            if (!string.IsNullOrEmpty(username))
            {
                SendMessage($"REGISTER,{username}");
                MessageBox.Show("User registered successfully!");

                // Navigate to LoginWindow
                InboxEmailWindow inboxEmailWindow = new InboxEmailWindow();
                inboxEmailWindow.Show();

                // Close the RegisterWindow
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a username.");
            }
        }

        private void SendMessage(string message)
        {
            using (UdpClient client = new UdpClient())
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ServerIp), Port);
                client.Send(data, data.Length, endPoint);
            }
        }
    }
}
