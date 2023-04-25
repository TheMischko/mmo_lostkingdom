using Shared.Utils;
using UnityEngine;

namespace Networking.MessageSenders {
    public static class MovementSender {
        private const int instruction = 20;

        public static void SendMessage(int movementType, Vector2 position, Vector2 direction, float speed) {
            BufferWriter bw = new BufferWriter();
            bw.WriteInt(instruction);
            bw.WriteInt(movementType);
            // Write position
            bw.WriteFloat(position.x);
            bw.WriteFloat(position.y);
            // Write direction
            bw.WriteFloat(position.x);
            bw.WriteFloat(position.y);
            //Write speed
            bw.WriteFloat(speed);
            
        }
    }
}