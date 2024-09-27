using MailApp.MVVM.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MailApp.Net
{
    public class Server
    {
        private UdpClient _udpClient;

        public Server(string ipAddress, int port)
        {
            _udpClient = new UdpClient();
            _udpClient.Connect(ipAddress, port);
        }

        public void SendData(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            _udpClient.Send(bytes, bytes.Length);
        }
    }
}
