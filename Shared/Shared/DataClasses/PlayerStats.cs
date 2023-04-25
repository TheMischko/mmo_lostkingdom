using Shared.Utils;

namespace Shared.DataClasses {
    public class PlayerStats:IBufferable {
        public float maxHealth;
        public float currentHealth;
        public float speed;

        public PlayerStats(float maxHealth, float currentHealth, float speed) {
            this.maxHealth = maxHealth;
            this.currentHealth = currentHealth;
            this.speed = speed;
        }

        public byte[] ToBytes() {
            BufferWriter bw = new BufferWriter();
            
            bw.WriteFloat(maxHealth);
            bw.WriteFloat(currentHealth);
            bw.WriteFloat(speed);
            
            return bw.ToArray();
        }

        public IBufferable FromBytes(BufferReader br) {
            float maxHealth = br.ReadFloat();
            float currentHealth = br.ReadFloat();
            float speed = br.ReadFloat();

            return new PlayerStats(maxHealth, currentHealth, speed);
        }
        
        public static PlayerStats FromBytesFactory(BufferReader br) {
            float maxHealth = br.ReadFloat();
            float currentHealth = br.ReadFloat();
            float speed = br.ReadFloat();

            return new PlayerStats(maxHealth, currentHealth, speed);
        }
    }
}