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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                UdpClient client = new UdpClient();
                string message = $"LOGIN|{username}|{password}";
                byte[] data = Encoding.UTF8.GetBytes(message);

                try
                {
                    client.Send(data, data.Length, "127.0.0.1", 11000); // Gửi đến server

                    // Nhận phản hồi từ server
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receivedData = client.Receive(ref remoteEndPoint);
                    string response = Encoding.UTF8.GetString(receivedData);

                    if (response == "SUCCESS")
                    {
                        MessageBox.Show("Login successful!");
                        // Open InboxSendWindow after successful login
                        InboxEmailWindow inboxEmailWindow = new InboxEmailWindow(username);
                        inboxEmailWindow.Show();
                        this.Close(); // Close the login window
                    }
                    else
                    {
                        MessageBox.Show("Login failed. Please check your username and password.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
                finally
                {
                    client.Close();
                }
            }
            else
            {
                MessageBox.Show("Please enter all fields.");
            }
        }
    }
}
