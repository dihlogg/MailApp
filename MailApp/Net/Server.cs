using MailApp.MVVM.Model;
using MailApp.Net.IO;
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
        //private UdpClient _udpClient;

        //public Server(string ipAddress, int port)
        //{
        //    _udpClient = new UdpClient();
        //    _udpClient.Connect(ipAddress, port);
        //}

        //public void SendData(string data)
        //{
        //    byte[] bytes = Encoding.UTF8.GetBytes(data);
        //    _udpClient.Send(bytes, bytes.Length);
        //}
        internal class MailServer
        {
            private UdpClient _udpClient;
            private IPEndPoint _serverEndPoint;

            public event Action emailSentEvent;
            public event Action emailReceivedEvent;

            public MailServer()
            {
                _udpClient = new UdpClient();
            }

            public void ConnectToMailServer(string serverIp, int serverPort, string emailAddress)
            {
                _serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
                SendRegistrationPacket(emailAddress);

                // Start listening for incoming packets
                StartListening();
            }

            // Send a registration packet to the server
            private void SendRegistrationPacket(string emailAddress)
            {
                var packetBuilder = new PacketBuilder();
                packetBuilder.WriteOpCode(0); // OpCode 0 for registration
                packetBuilder.WriteMessage(emailAddress);

                byte[] packetBytes = packetBuilder.GetPacketBytes();
                _udpClient.Send(packetBytes, packetBytes.Length, _serverEndPoint);
            }

            // Method to send an email to the server
            public void SendEmailToServer(Email email)
            {
                var packetBuilder = new PacketBuilder();
                packetBuilder.WriteOpCode(1); // OpCode 1 for sending an email
                packetBuilder.WriteEmail(email); // Use the PacketBuilder's WriteEmail method

                byte[] packetBytes = packetBuilder.GetPacketBytes();
                _udpClient.Send(packetBytes, packetBytes.Length, _serverEndPoint);

                // Trigger emailSentEvent after sending
                emailSentEvent?.Invoke();
            }

            private void StartListening()
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        IPEndPoint remoteEndPoint = null;
                        byte[] receivedData = _udpClient.Receive(ref remoteEndPoint);

                        ProcessReceivedData(receivedData);
                    }
                });
            }

            private void ProcessReceivedData(byte[] data)
            {
                PacketReader packetReader = new PacketReader(new MemoryStream(data));
                var opcode = packetReader.ReadByte();

                switch (opcode)
                {
                    case 2:
                        var receivedEmail = packetReader.ReadEmail();
                        Console.WriteLine($"New Email from {receivedEmail.Sender}: {receivedEmail.Subject}");
                        emailReceivedEvent?.Invoke();
                        break;

                    default:
                        Console.WriteLine("Unknown opcode received");
                        break;
                }
            }
        }
    }
}
