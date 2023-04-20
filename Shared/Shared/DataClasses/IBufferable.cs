using Shared.Utils;

namespace Shared.DataClasses {
    public interface IBufferable {
        /**
         * Transfers instance to bytes.
         */
        byte[] ToBytes();

        /**
         * Builds the class from bytes.
         */
        IBufferable FromBytes(BufferReader br);
    }
}