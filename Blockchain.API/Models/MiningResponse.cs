using System.Collections.Generic;
using Blockchain.Lib;
using System;

namespace Blockchain.API.Controllers
{
    public class MiningResponse
    {
        public string Message { get; set; }
        public int Index { get; set; }
        public List<Transaction> Transactions { get; set; }
        public long Proof { get; set; }
        public string PreviousHash { get; set; }
        public TimeSpan CalculationTime { get; set; }
    }
}