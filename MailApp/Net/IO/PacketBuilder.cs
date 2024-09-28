using MailApp.MVVM.Model;
using System.IO;
using System.Text;

namespace MailApp.Net.IO
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
            _ms.Write(BitConverter.GetBytes(strBytes.Length), 0, 4); // Write length of string
            _ms.Write(strBytes, 0, strBytes.Length); // Write string content
        }

        public void WriteEmail(Email email)
        {
            WriteMessage(email.Subject);
            WriteMessage(email.Sender);
            WriteMessage(email.Body);
            WriteDateTime(email.ReceivedDate); // Custom method for DateTime
        }

        private void WriteDateTime(DateTime dateTime)
        {
            var ticks = dateTime.Ticks;
            _ms.Write(BitConverter.GetBytes(ticks), 0, 8); // Write DateTime as ticks
        }

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}