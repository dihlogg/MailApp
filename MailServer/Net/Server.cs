using MailServer.MVVM.Model;
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
using static System.Net.Mime.MediaTypeNames;

namespace MailServer.Net
{
    public class Server
    {
        private UdpClient _udpClient;

        public Server(int port)
        {
            _udpClient = new UdpClient(port);
        }

        public void StartListening()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    IPEndPoint remoteEndPoint = null;
                    byte[] bytes = _udpClient.Receive(ref remoteEndPoint);
                    string data = Encoding.UTF8.GetString(bytes);
                }
            });
        }
    }
}
