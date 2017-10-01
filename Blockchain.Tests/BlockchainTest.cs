using Blockchain.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Blockchain.Tests
{
    [TestClass]
    public class BlockchainTest
    {
        [TestMethod]
        public void CreateNewBlockchainAndAddNewTransaction()
        {
            Blockchain.Lib.Blockchain blockchain = new Lib.Blockchain();
            var newBlockIndex = blockchain.NewTransaction(new Lib.Transaction
            {
                Sender = "123123",
                Recipient = "4564564645",
                Amount = 100.5
            });

            Assert.AreEqual(2, newBlockIndex);
        }

        [TestMethod]
        public void TestHashesOfTheIdenticalBlocks()
        {
            Block b1 = new Block
            {
                Index = 0,
                PreviousHash = "asdasdasd",
                Proof = 1,
                Timestamp = DateTime.MinValue,
                Transactions = new System.Collections.Generic.List<Transaction>()
            };

            Block b2 = new Block
            {
                PreviousHash = "asdasdasd",
                Timestamp = DateTime.MinValue,
                Proof = 1,
                Transactions = new System.Collections.Generic.List<Transaction>(),
                Index = 0
            };

            Assert.AreEqual(Blockchain.Lib.Blockchain.Hash(b1), Blockchain.Lib.Blockchain.Hash(b2));
        }
    }
}
