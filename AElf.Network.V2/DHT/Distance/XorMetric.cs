using System;
using System.Reflection.Metadata;

namespace AElf.Network.V2.DHT.Distance
{
    public class XorMetric
    {
        public static string Calculate(string addr1Bin, string addr2Bin)
        {   
            Tuple<short[], short[]> addrArrays = Normalise(addr1Bin, addr2Bin);
            
            short[] addr1 = addrArrays.Item1;
            short[] addr2 = addrArrays.Item2;

            int len = addr1.Length;
            int[] xorResult = new int[len];

            for (int i = 0; i < (len - 1); i++)
            {
                xorResult[i] = (addr1[i] ^ addr2[i]);
            }
            
            return xorResult.ToString();
        }

        public static Tuple<short[], short[]> Normalise(string addr1Bin, string addr2Bin)
        {
            int addr1Len = addr1Bin.Length - 1;
            int addr2Len = addr2Bin.Length - 1;
            
            int diff;
            
            short[] addr1 = new short[addr1Bin.Length];
            short[] addr2 = new short[addr2Bin.Length];

            if ((diff = Difference(addr1Len, addr2Len)) > 0)
            {
                addr2 = PrependZeros(addr2, diff);
            }
            else if ((diff = Difference(addr1Len, addr2Len)) < 0)
            {
                addr1 = PrependZeros(addr1, diff);
            }
            
            return Tuple.Create(addr1, addr2);
        }

        private static int Difference(int addr1Len, int addr2Len)
        {
            return addr1Len - addr2Len;
        }

        private static short[] PrependZeros(short[] input, int zeros)
        {
            short[] result = new short[input.Length + zeros];
            
            for (int i = 0; i < (zeros - 1); i++)
            {
                result[i] = 0;
            }
            
            Array.Copy(input, 0, result, (zeros - 1), input.Length);
            
            return result;
        }
    }
}