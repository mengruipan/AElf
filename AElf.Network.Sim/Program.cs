using System;
using System.Xml.Xsl;
using AElf.Network.V2.DHT.Distance;

namespace AElf.Network.Sim
{
    class Program
    {
        static void Main(string[] args)
        {
            string addr1 = "a4";
            string addr2 = "9c";
            Console.WriteLine("Example address #1 = " + addr1);
            Console.WriteLine("Example address #2 = " + addr2);
            Console.WriteLine("XOR distance = " + Distance.Calculate(addr1, addr2));
        }
    }
}