using System;
using System.Collections.Generic;
using AElf.Cryptography.ECDSA;
using AElf.Kernel;
using AElf.Types.CSharp;
using Akka.Util.Internal.Collections;
using Google.Protobuf;
using Org.BouncyCastle.Security;

namespace AElf.Benchmark
{
    public class TransactionDataGenerator
    {
        private int _totalNumber;
        public Dictionary<Hash, ECKeyPair> KeyDict;
        public Dictionary<Hash, ECKeyPair> Target;
        public List<Hash> AllHashs { get; }

        public TransactionDataGenerator(int maxNumber, int maxGroup)
        {
            _totalNumber = maxNumber;
            KeyDict = new Dictionary<Hash, ECKeyPair>();
            Target = new Dictionary<Hash, ECKeyPair>();
            for (int i = 0; i < maxNumber + 90; i++)
            {
                if (maxNumber > 100 && i % (maxNumber / 10) == 0)
                {
                    Console.WriteLine((double)(i*100) / (double)maxNumber + "% pub-priv key generated");
                }
                var keyPairGenerator = new KeyPairGenerator();
                var kpair = keyPairGenerator.Generate();
                KeyDict.Add(new Hash(kpair.GetAddress()), kpair);
            }

            for (int i = 0; i < maxGroup + 2; i++)
            {
                var keyPairGenerator = new KeyPairGenerator();
                var kpair = keyPairGenerator.Generate();
                Target.Add(new Hash(kpair.GetAddress()), kpair);
            }
            
            AllHashs = new List<Hash>();
            AllHashs.AddRange(KeyDict.Keys);
            AllHashs.AddRange(Target.Keys);
        }

        private IEnumerable<KeyValuePair<Hash, Hash>> GenerateTransferAddressPair(int txCount, double conflictRate, ref Iterator<KeyValuePair<Hash, ECKeyPair>> keyDictIter, ref Iterator<KeyValuePair<Hash, ECKeyPair>> conflictKeyIter)
        {
            if (txCount > _totalNumber) throw new InvalidParameterException();
            var txAccountList = new List<KeyValuePair<Hash, Hash>>();
            
            int conflictTxCount = (int) (conflictRate * txCount);
            var conflictKeyPair = conflictKeyIter.Next();
            var conflictAddr = new Hash(conflictKeyPair.Key);

            
            for (int i = 0; i < conflictTxCount; i++)
            {
                var senderKp = keyDictIter.Next();
                txAccountList.Add(new KeyValuePair<Hash, Hash>(senderKp.Key, conflictAddr));
            }

            for (int i = 0; i < txCount - conflictTxCount; i++)
            {
                var senderKp = keyDictIter.Next();
                var receiverKp = keyDictIter.Next();
                txAccountList.Add(new KeyValuePair<Hash, Hash>(senderKp.Key, receiverKp.Key));
            }

            return txAccountList;
        }

        public List<ITransaction> GetMultipleGroupTx(int txNumber, int groupCount, Hash contractAddr)
        {
            if(txNumber > _totalNumber)  throw new InvalidParameterException();
            int txNumPerGroup = txNumber / groupCount;
            var keyDictIter = KeyDict.Iterator();
            var conflictKeyIter = Target.Iterator();
            List<ITransaction> txList = new List<ITransaction>();
            for (int i = 0; i < groupCount; i++)
            {
                var addrPair = GenerateTransferAddressPair(txNumPerGroup, 1, ref keyDictIter, ref conflictKeyIter);
                var groupTxList = GenerateTransferTransactions(contractAddr, addrPair);
                txList.AddRange(groupTxList);
            }

            return txList;
        }
        
        public List<ITransaction> GenerateTransferTransactions(Hash tokenContractAddr, IEnumerable<KeyValuePair<Hash, Hash>> transferAddressPairs)
        {
            var resList = new List<ITransaction>();
            foreach (var addressPair in transferAddressPairs)
            {

                var keyPair = KeyDict[addressPair.Key];
                ulong qty = 50;
                Transaction tx = new Transaction()
                {
                    From = addressPair.Key,
                    To = tokenContractAddr,
                    IncrementId = 0,
                    MethodName = "Transfer",
                    Params = ByteString.CopyFrom(ParamsPacker.Pack(addressPair.Key, addressPair.Value, (ulong)20)),
                    P = ByteString.CopyFrom(keyPair.PublicKey.Q.GetEncoded())
                };

                Hash txHash = tx.GetHash();
                
                ECSigner signer = new ECSigner();
                ECSignature signature = signer.Sign(keyPair, txHash.GetHashBytes());

                tx.R = ByteString.CopyFrom(signature.R);
                tx.S = ByteString.CopyFrom(signature.S);
                
                resList.Add(tx);
            }

            return resList;
        }
    }
}