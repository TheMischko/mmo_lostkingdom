using Shared.Utils;

namespace Shared.DataClasses {
    public class PlayerData:IBufferable {
        public string name;
        public float posX;
        public float posY;
        public string sprite;
        public PlayerStats stats;

        public PlayerData(string name, float posX, float posY, string sprite, PlayerStats stats) {
            this.name = name;
            this.posX = posX;
            this.posY = posY;
            this.sprite = sprite;
            this.stats = stats;
        }
        
        public byte[] ToBytes() {
            BufferWriter bw = new BufferWriter();
            bw.WriteString(name);
            bw.WriteFloat(posX);
            bw.WriteFloat(posY);
            bw.WriteString(sprite);
            bw.AddData(stats.ToBytes());
            return bw.ToArray();
        }

        public IBufferable FromBytes(BufferReader br) {
            string name = br.ReadString();
            float posX = br.ReadFloat();
            float posY = br.ReadFloat();
            string sprite = br.ReadString();
            PlayerStats stats = PlayerStats.FromBytesFactory(br);

            return new PlayerData(name, posX, posY, sprite, stats);
        }

        public static PlayerData FromBytesFactory(BufferReader br) {
            string name = br.ReadString();
            float posX = br.ReadFloat();
            float posY = br.ReadFloat();
            string sprite = br.ReadString();
            PlayerStats stats = PlayerStats.FromBytesFactory(br);

            return new PlayerData(name, posX, posY, sprite, stats);
        }
    }
}