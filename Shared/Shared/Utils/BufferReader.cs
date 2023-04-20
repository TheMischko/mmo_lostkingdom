using System;
using System.IO;

namespace Shared.Utils {
    public class BufferReader : IDisposable
    {
        private MemoryStream memoryStream;
        private BinaryReader reader;
        private BinaryWriter writer;

        public BufferReader(byte[] data)
        {
            memoryStream = new MemoryStream(data);
            reader = new BinaryReader(memoryStream);
            writer = new BinaryWriter(memoryStream);
        }

        public byte[] ToArray()
        {
            return memoryStream.ToArray();
        }

        public int ReadInt()
        {
            return reader.ReadInt32();
        }

        public string ReadString()
        {
            return reader.ReadString();
        }

        public byte[] ReadBytes(int count) {
            return reader.ReadBytes(count);
        }

        public DateTime ReadDatetime() {
            long dtBinary= reader.ReadInt64();
            return DateTime.FromBinary(dtBinary);
        }

        public void Dispose()
        {
            reader.Dispose();
            writer.Dispose();
            memoryStream.Dispose();
        }
    }
}