using System.Collections.Generic;
using System.Linq;
using Windows.UI;

namespace NextBusDesktop.Utilities
{
    public static class HexToRgb
    {
        public static Color HexToColor(string hex)
        {
            string hexString = hex[0] == '#' ? hex.TrimStart('#') : hex;
            uint value = uint.Parse(hexString, System.Globalization.NumberStyles.HexNumber);

            return Color.FromArgb(
                a: 0xFF, 
                r: (byte)((value >> 16) & 0xFF), 
                g: (byte)((value >> 8) & 0xFF), 
                b: (byte)(value & 0xFF)
            );
        }
    }
}
