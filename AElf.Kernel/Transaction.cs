using AElf.Kernel.Extensions;
using System;

namespace AElf.Kernel
{
    public class Transaction : ITransaction
    {
        public Transaction() { }

        public IHash<ITransaction> GetHash()
        {
            return new Hash<ITransaction>(this.CalculateHash());
        }

        public ITransactionParallelMetaData GetParallelMetaData()
        {
            throw new NotImplementedException();
        }

        public string MethodName { get; set; }
        public object[] Params { get; set; }
        public IAccount From { get; set; }
        public IAccount To { get; set; }
        public ulong IncrementId { get; set; }

        public IHash<IBlockHeader> LastBlockHashWhenCreating()
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
