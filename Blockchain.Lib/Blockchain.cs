using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain.Lib
{
    public class Blockchain : IDisposable
    {
        private string PoWResult;
        private readonly Guid node_identifier = Guid.NewGuid();

        public List<Block> Chain { get; private set; }
        private List<Transaction> CurrentTransactions { get; set; }

        private HashAlgorithm algorithm { get; set; }

        private Block LastBlock
        {
            get
            {
                return Chain[Chain.Count - 1];
            }
        }

        public Blockchain(string startingHashStringToCheckForValidBlock = "000")
        {
            Chain = new List<Block>();
            CurrentTransactions = new List<Transaction>();
            //Create genesis block
            this.NewBlock(100, "1");

            algorithm = SHA256.Create();
            PoWResult = startingHashStringToCheckForValidBlock;
        }


        /// <summary>
        /// Creates a new transaction to go into the next mined Block
        /// </summary>
        /// <param name="transaction">Transaction Information</param>
        /// <returns>The index of the Block that will hold this transaction</returns>
        public int NewTransaction(Transaction transaction)
        {
            this.CurrentTransactions.Add(transaction);

            return this.LastBlock.Index + 1;
        }

        /// <summary>
        /// Mines the new block using PoW
        /// </summary>
        /// <returns>Returns the mined block</returns>
        public Block MineNewBlock()
        {
            //Run PoW to get new proof
            var last_block = this.LastBlock;
            var last_proof = last_block.Proof;
            var newProof = this.ProofOfWork(last_proof);

            //Get the coin reward
            NewTransaction(new Lib.Transaction
            {
                Sender = "0",
                Recipient = node_identifier.ToString(),
                Amount = 1
            });

            var block = NewBlock(newProof);

            return block;
        }


        /// <summary>
        ///  Creates a new block and appends it to the block chain
        /// </summary>
        /// <param name="proof">The proof given by the Proof of Work algorithm</param>
        /// <param name="previous_hash">(Optional)Hash of the previous block</param>
        /// <returns>New Block</returns>
        private Block NewBlock(long proof, string previous_hash = null)
        {
            //create the new block
            var newBlock = new Block
            {
                Index = Chain.Count + 1,
                Timestamp = DateTime.Now,
                Transactions = new List<Transaction>(CurrentTransactions),
                Proof = proof,
                PreviousHash = previous_hash ?? Blockchain.Hash(LastBlock)
            };

            //reset the transactions
            CurrentTransactions.Clear();

            Chain.Add(newBlock);
            return newBlock;
        }


        /// <summary>
        /// To create or mine a new block proof should be find. Proof should be hard to discover, easy to verify.
        /// </summary>
        /// <param name="lastProof"></param>
        /// <returns>Proof</returns>
        private long ProofOfWork(long lastProof)
        {
            long proof = 0;
            while (!ValidateProof(proof, lastProof))
                proof++;

            return proof;
        }

        /// <summary>
        /// This is a simple PoW algorithm which concatenates two proofs and hashes them. 
        /// If leading certain number of characters(length of PoWResult) is same as PoWResult string, it is validated.
        /// </summary>
        /// <param name="proof">Current proof to be tested</param>
        /// <param name="lastProof">Last block's proof</param>
        /// <returns>Validated or Not</returns>
        private bool ValidateProof(long proof, long lastProof)
        {
            var newKey = proof.ToString() + lastProof.ToString();
            var hash = Convert.ToBase64String(algorithm.ComputeHash(Encoding.ASCII.GetBytes(newKey)));

            return hash.Substring(0, PoWResult.Length) == PoWResult;
        }


        /// <summary>
        /// Creates SHA-256 hash of a block
        /// </summary>
        /// <param name="block">Block</param>
        /// <returns></returns>
        private static string Hash(Block block)
        {
            var blockJson = JsonConvert.SerializeObject(block);
            using (var algorithm = SHA256.Create())
            {
                //Create the hash of the json
                return Convert.ToBase64String(algorithm.ComputeHash(Encoding.ASCII.GetBytes(blockJson)));
            }
        }

        #region Consensus
        /// <summary>
        /// This is our Consensus Algorithm, it resolves conflicts
        /// by replacing our chain with the longest one in the network.
        /// </summary>
        /// <returns></returns>
        public bool ResolveConflict(List<List<Block>> neighbours)
        {
            List<Block> new_chain = null;
            int maxChainLength = this.Chain.Count;

            foreach (var node in neighbours)
            {
                if (node.Count > maxChainLength && ValidateChain(node))
                {
                    maxChainLength = node.Count;
                    new_chain = node;
                }
            }

            if (new_chain != null)
            {
                this.Chain = new_chain;
                return true;
            }
            return false;
        }

        /// <summary>
        ///  Determine if a given blockchain is valid
        /// </summary>
        /// <param name="chain">Blockchain</param>
        /// <returns></returns>
        private bool ValidateChain(List<Block> chain)
        {
            var last_block = chain[0];
            for (int i = 1; i < chain.Count; i++)
            {
                var block = chain[i];
                if (block.PreviousHash != Hash(last_block))
                    return false;

                if (!ValidateProof(block.Proof, last_block.Proof))
                    return false;

                last_block = block;
            }

            return true;
        }
        #endregion


        public void Dispose()
        {
            algorithm.Dispose();
        }
    }
}
