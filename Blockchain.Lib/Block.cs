using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Blockchain.Lib
{
    /// <summary>
    /// The Block Object which stores transactions and info about the previous block
    /// Properties are ordered to ensure consistent hash output
    /// </summary>
    public class Block
    {
        [JsonProperty(Order = 1)]
        public int Index { get; set; }
        [JsonProperty(Order = 2)]
        public DateTime Timestamp { get; set; }
        [JsonProperty(Order = 3)]
        public List<Transaction> Transactions { get; set; }
        [JsonProperty(Order = 4)]
        public long Proof { get; set; }
        [JsonProperty(Order = 5)]
        public string PreviousHash { get; set; }//Containing the hash of the previous block is what makes blockhain immutable
    }
}
