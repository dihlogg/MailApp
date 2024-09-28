using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MailServer.Net
{
    public class Client
    {
        private UdpClient _udpClient;

        public Client(string serverIp, int serverPort)
        {
            _udpClient = new UdpClient();
            _udpClient.Connect(IPAddress.Parse(serverIp), serverPort);
        }

        public void CreateAccount(string email)
        {
            string message = $"Account Created|{email}";
            byte[] data = Encoding.UTF8.GetBytes(message);
            _udpClient.Send(data, data.Length);
        }
    }
}
