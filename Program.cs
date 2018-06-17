using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WavesCS;
using HashLib;
using Blake2Sharp;
using curve25519;

namespace LearnWaves
{
    class Program
    {
        static void Main(string[] args)
        {
            AtomicSwap testSwap = new AtomicSwap
                (
                senderPrivateKey: "CMLwxbMZJMztyTJ6Zkos66cgU7DybfFJfyJtTVpme54t",
                receiverPrivateKey: "25Um7fKYkySZnweUEVAn9RLtxN5xHRd7iqpqYSMNQEeT",
                secret: "secret?????"
                );

            testSwap.SetSmartContract(scriptAccountPrivateKey: new byte[] { 1,2,3,4} , lockHeight: 233); 
        }



        public class AtomicSwap
        {

            public AtomicSwap(string senderPrivateKey, string receiverPrivateKey, string secret)
            {
                SenderAccount = PrivateKeyAccount.CreateFromPrivateKey(senderPrivateKey, AddressEncoding.TestNet);
                ReceiverAccount = PrivateKeyAccount.CreateFromPrivateKey(receiverPrivateKey, AddressEncoding.TestNet);
                Secret = secret;
            }

            private PrivateKeyAccount SenderAccount { get; }
            private PrivateKeyAccount ReceiverAccount { get; }
            private string Secret { get; }

            public void SetSmartContract(byte[] scriptAccountPrivateKey, int lockHeight)
            {
                string smartContract = "match tx {n" +
        "case t : SetScriptTransaction => truen" +
        "case _ => n" +
            "let pubKeyUser1 = base58'" + Base58.Encode(SenderAccount.PublicKey)+ "'n" +
            "let pubKeyUser2 = base58'" + Base58.Encode(ReceiverAccount.PublicKey) + "'n" +
            "let lockHeight = " + lockHeight.ToString() + "n" +
            "let hashedSecret = tx.proofs[1]n" +
            "let preimage = tx.proofs[2]n" +
            "let hashPreimage = sha256(preimage)n" +
            "let secretMatches = hashPreimage == base58'" + Secret + "'n" +
            "let signedByUser1 = sigVerify(tx.bodyBytes, tx.proofs[0], pubKeyUser1)n" +
            "let signedByUser2 = sigVerify(tx.bodyBytes, tx.proofs[0], pubKeyUser2)n" +
            "let afterTimelock = height > lockHeightn" +
            "(signedByUser2 && afterTimelock) || (signedByUser1 && secretMatches)n" +
"}";
            }

     // and then do other stuff....

        }
    }
}
