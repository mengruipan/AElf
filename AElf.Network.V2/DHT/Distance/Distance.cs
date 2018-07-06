using AElf.Network.V2.DHT.Distance.Helpers;

namespace AElf.Network.V2.DHT.Distance
{
    public class Distance
    {
        /// <summary>
        /// Calculates the distance between two hexademical addresses.
        /// </summary>
        /// <param name="addr1Hex"></param>
        /// <param name="addr2Hex"></param>
        /// <returns>Distance as an integer</returns>
        public static int Calculate(string addr1Hex, string addr2Hex)
        {
            // convert hex addresses to binary
            string addr1Bin = Converter.HexToBin(addr1Hex);
            string addr2Bin = Converter.HexToBin(addr2Hex);
            
            string xorResult = XorMetric.Compute(addr1Bin, addr2Bin); // compute bitwise XOR
            return Converter.BinToInt(xorResult); // convert & return xorResult as an integer
        }
    }
}