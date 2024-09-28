using MailServer.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MailServer.Net.IO
{
    internal class PacketReader : BinaryReader
    {
        public PacketReader(Stream input) : base(input) { }

        public string ReadMessage()
        {
            int length = ReadInt32();
            byte[] msgBuffer = ReadBytes(length);
            return Encoding.UTF8.GetString(msgBuffer);
        }

        public Email ReadEmail()
        {
            var subject = ReadMessage();
            var sender = ReadMessage();
            var body = ReadMessage();
            var receivedDateTicks = ReadInt64();
            var receivedDate = new DateTime(receivedDateTicks);

            return new Email
            {
                Subject = subject,
                Sender = sender,
                Body = body,
                ReceivedDate = receivedDate
            };
        }
    }
}
