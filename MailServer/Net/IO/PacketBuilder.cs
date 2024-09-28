using MailServer.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailServer.Net.IO
{
    internal class PacketBuilder
    {
        private MemoryStream _ms;

        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }

        public void WriteMessage(string str)
        {
            var strBytes = Encoding.UTF8.GetBytes(str);
            _ms.Write(BitConverter.GetBytes(strBytes.Length), 0, 4); 
            _ms.Write(strBytes, 0, strBytes.Length); 
        }

        public void WriteEmail(Email email)
        {
            WriteMessage(email.Subject);
            WriteMessage(email.Sender);
            WriteMessage(email.Body);
            WriteDateTime(email.ReceivedDate); 
        }

        private void WriteDateTime(DateTime dateTime)
        {
            var ticks = dateTime.Ticks;
            _ms.Write(BitConverter.GetBytes(ticks), 0, 8);
        }

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}
