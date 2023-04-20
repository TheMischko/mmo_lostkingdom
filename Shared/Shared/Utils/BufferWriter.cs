using System.IO;

namespace Shared.Utils {
    public class BufferWriter {
        private MemoryStream memoryStream;
        private BinaryWriter writer;

        public BufferWriter()
        {
            memoryStream = new MemoryStream();
            writer = new BinaryWriter(memoryStream);
        }

        public byte[] ToArray()
        {
            return memoryStream.ToArray();
        }

        public void AddData(byte[] data) {
            writer.Write(data);
        }

        public void WriteInt(int value)
        {
            writer.Write(value);
        }

        public void WriteString(string value)
        {
            writer.Write(value);
        }

        public void Dispose()
        {
            writer.Dispose();
            memoryStream.Dispose();
        }
    }
}