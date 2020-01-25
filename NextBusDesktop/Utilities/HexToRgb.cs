using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace NextBusDesktop.Utilities
{
    public static class HexToRgb
    {
        private const string _base16Digits = "0123456789ABCDEF";
        private const string _charDigits = "ABCDEF";

        public static Color HexToColor(string hex)
        {
            hex = hex.TrimStart('#').ToUpper();

            if (hex.Length != 6 || !hex.All(d => _base16Digits.Contains(d)))
                return new Color();

            var bytes = new List<byte>(3);
            int i = 0;
            while (i < hex.Length)
            {
                int b = Parse0ToF(hex[i + 1]); // b = x * 16^0
                int a = Parse0ToF(hex[i]) * 16; // b = x * 16^1
                bytes.Add((byte)(b + a));
                i += 2;
            }

            return Color.FromArgb(255, bytes[0], bytes[1], bytes[2]);
        }

        /// <summary>
        /// Parses a single hex digit value from 0 to F
        /// </summary>
        private static int Parse0ToF(char character)
        {
            if (int.TryParse(character.ToString(), out int result)) return result;
            else return ParseAToF(character);
        }

        /// <summary>
        /// Parses a single digit value from A to F
        /// </summary>
        private static int ParseAToF(char character)
        {
            bool isIndexFound = false;
            int i = -1;
            do
            {
                i++;
                if (_charDigits[i] == character) 
                    isIndexFound = true;
            } while (i < _charDigits.Length && !isIndexFound);

            return i + 10; // Offset i by the number of integer digit values that can be represented (0-9)
        }
    }
}
