using Blockchain.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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
        public void ConsensusAlgorithmForSameChainsTest()
        {
            Lib.Blockchain blockchain1 = new Lib.Blockchain();
            Lib.Blockchain blockchain2 = new Lib.Blockchain();
            Lib.Blockchain blockchain3 = new Lib.Blockchain();

            List<List<Block>> neighbourChains = new List<List<Block>>();
            neighbourChains.Add(blockchain3.Chain);
            neighbourChains.Add(blockchain2.Chain);

            Assert.IsFalse(blockchain1.ResolveConflict(neighbourChains));
        }

        [TestMethod]
        public void ConsensusAlgorithmForDifferentChainsTest()
        {
            string hashToCompare = "0";//determines the difficulty of mining
            Lib.Blockchain blockchain1 = new Lib.Blockchain(hashToCompare);
            Lib.Blockchain blockchain2 = new Lib.Blockchain(hashToCompare);
            Lib.Blockchain blockchain3 = new Lib.Blockchain(hashToCompare);

            blockchain2.MineNewBlock();

            List<List<Block>> neighbourChains = new List<List<Block>>();
            neighbourChains.Add(blockchain3.Chain);
            neighbourChains.Add(blockchain2.Chain);

            Assert.IsTrue(blockchain1.ResolveConflict(neighbourChains));
            Assert.IsTrue(blockchain1.Chain.Count == 2);
        }
    }
}
