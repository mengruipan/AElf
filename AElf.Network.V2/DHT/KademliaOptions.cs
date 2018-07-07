namespace AElf.Network.V2.DHT
{
    public class KademliaOptions
    {
        public const int Alpha = 3;
        public const int BucketSize = 16;
        public const int KeySize = 256;
        public const int MaxSteps = 8;
        
        public const long ReqTimeout = 300;
        public const long BucketRefresh = 7200;
        public const long DiscoveryCycle = 30;
    }
}