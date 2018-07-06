using System;

namespace AElf.Network.V2.DHT.Distance.Helpers
{
    public class Converter
    {
        public static string HexToBin(string hex)
        {
            return Convert.ToString(Convert.ToInt32(hex, 16), 2);
        }
        
        public static int BinToInt(string binary)
        {
            return Convert.ToInt32(binary, 2);
        }
    }
}