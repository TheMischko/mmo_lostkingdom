using UnityEngine;

namespace Chat {
    public static class ChatColors {
        public static Color dateColor = GetColorRGBA(168,175,184, 1f);
        public static Color userColor = GetColorRGBA(102, 0, 102, 1f);
        public static Color messageColor = GetColorRGBA(238, 238, 238, 1f);
        public static Color adminColor = GetColorRGBA(107, 228, 223, 1f);
        public static Color serverColor = GetColorRGBA(250, 128, 114, 1f);

        private static Color GetColorRGBA(int red, int green, int blue, float alpha) {
            return new Color((float)red / 255, (float)green / 255, (float)blue / 255, alpha);
        }
    }
}