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
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailServer.Net.IO;

namespace MailServer.Net
{
    public class Server
    {
        private UdpClient _udpServer;
        private IPEndPoint _clientEndPoint;

        public Server(int port)
        {
            _udpServer = new UdpClient(port);
        }

        public void StartListening()
        {
            Console.WriteLine("Mail server started, waiting for packets...");

            Task.Run(() =>
            {
                while (true)
                {
                    _clientEndPoint = null;
                    byte[] receivedData = _udpServer.Receive(ref _clientEndPoint);

                    ProcessReceivedPacket(receivedData);
                }
            });
        }

        private void ProcessReceivedPacket(byte[] data)
        {
            using (var packetReader = new PacketReader(new MemoryStream(data)))
            {
                var opcode = packetReader.ReadByte();

                switch (opcode)
                {
                    case 0:
                        var emailAddress = packetReader.ReadMessage();
                        Console.WriteLine($"{emailAddress} registered successfully.");
                        break;

                    case 1:
                        var email = packetReader.ReadEmail();
                        Console.WriteLine($"Email received from {email.Sender}: {email.Subject}");
                        break;

                    default:
                        Console.WriteLine("Unknown opcode received on server.");
                        break;
                }
            }
        }

        public void SendEmailReceivedNotification(IPEndPoint clientEndPoint, Email email)
        {
            var packetBuilder = new PacketBuilder();
            packetBuilder.WriteOpCode(2);
            packetBuilder.WriteEmail(email);

            byte[] packetBytes = packetBuilder.GetPacketBytes();
            _udpServer.Send(packetBytes, packetBytes.Length, clientEndPoint);
        }
    }
}

