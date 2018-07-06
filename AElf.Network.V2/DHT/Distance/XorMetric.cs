using System;
using System.Reflection.Metadata;

namespace AElf.Network.V2.DHT.Distance
{
    public class XorMetric
    {
        /// <summary>
        /// Computes the bitwise XOR of its inputs.
        /// </summary>
        /// <param name="addr1Bin"></param>
        /// <param name="addr2Bin"></param>
        /// <returns>Binary string-formatted XOR result</returns>
        public static string Compute(string addr1Bin, string addr2Bin)
        {   
            Tuple<short[], short[]> addrArrays = Normalise(addr1Bin, addr2Bin);
            
            short[] addr1 = addrArrays.Item1;
            short[] addr2 = addrArrays.Item2;

            int len = addr1.Length;
            int[] xorResult = new int[len];

            // store XOR result in array
            for (int i = 0; i < (len - 1); i++)
            {
                xorResult[i] = (addr1[i] ^ addr2[i]);
            }
            
            return xorResult.ToString(); // return XOR result as binary string
        }

        /// <summary>
        /// Converts the two binary string-formatted addresses to arrays
        /// and ensures they are the same length (number of bits).
        /// </summary>
        /// <param name="addr1Bin"></param>
        /// <param name="addr2Bin"></param>
        /// <returns>Tuple containing the resulting arrays</returns>
        public static Tuple<short[], short[]> Normalise(string addr1Bin, string addr2Bin)
        {
            int addr1Len = addr1Bin.Length - 1;
            int addr2Len = addr2Bin.Length - 1;
            
            int diff;
            
            short[] addr1 = new short[addr1Bin.Length];
            short[] addr2 = new short[addr2Bin.Length];

            if ((diff = addr1Len - addr2Len) > 0) // if addr1 had more bits than addr2
            {
                addr2 = PrependZeros(addr2, diff); // prepend zeros to addr2 so it matches addr1's length
            }
            else if ((diff = addr2Len - addr1Len) < 0) // if addr2 had more bits than addr1
            {
                addr1 = PrependZeros(addr1, diff); // prepend zeros to addr1 so it matches addr2's length
            }
            
            return Tuple.Create(addr1, addr2);
        }

        /// <summary>
        /// Prepends zeros the required number of zeros to the
        /// input array.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="zeros"></param>
        /// <returns>Array with the prepended zeros</returns>
        private static short[] PrependZeros(short[] input, int zeros)
        {
            short[] result = new short[input.Length + zeros];
            
            // add each required zero to the array
            for (int i = 0; i < (zeros - 1); i++)
            {
                result[i] = 0;
            }
            
            Array.Copy(input, 0, result, (zeros - 1), input.Length); // append the input array to the result array
            
            return result;
        }
    }
}