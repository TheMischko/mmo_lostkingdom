using Shared.Utils;

namespace Shared.DataClasses {
    public class UserData:IBufferable {
        public int index;
        public string name;

        public UserData(int index, string name) {
            this.index = index;
            this.name = name;
        }
        
        public byte[] ToBytes() {
            BufferWriter bw = new BufferWriter();
            bw.WriteInt(index);
            bw.WriteString(name);
            return bw.ToArray();
        }
        
        public IBufferable FromBytes(BufferReader br) {
            int index = br.ReadInt();
            string name = br.ReadString();
            return new UserData(index, name);
        }

        public static UserData FromBytesFactory(BufferReader br) {
            int index = br.ReadInt();
            string name = br.ReadString();
            return new UserData(index, name);
        }
    }
}