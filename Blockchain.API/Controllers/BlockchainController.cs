using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blockchain.API.Controllers
{
    [Produces("application/json")]
    [Route("/")]
    public class BlockchainController : Controller
    {
        [Route("/mine")]
        [HttpGet]
        public MiningResponse Mine()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var block = BlockchainObject.Instance.MineNewBlock();
            stopwatch.Stop();
            return new MiningResponse
            {
                Message = "New block forged",
                Index = block.Index,
                Transactions = block.Transactions,
                Proof = block.Proof,
                PreviousHash = block.PreviousHash,
                CalculationTime = stopwatch.Elapsed
            };
        }

        [Route("/transactions/new")]
        [HttpPost]
        public ActionResult New_Transaction(TransactionRequest request)
        {
            if (ModelState.IsValid && request != null)
            {

                var index = BlockchainObject.Instance.NewTransaction(new Lib.Transaction
                {
                    Amount = request.Amount,
                    Recipient = request.Recipient,
                    Sender = request.Sender
                });

                return Content(String.Format("Transaction will be added to block {0}", index));
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("/chain")]
        [HttpGet]
        public ChainResponse GetFullChain()
        {
            var chainResponse = new ChainResponse
            {
                chain = BlockchainObject.Instance.Chain,
                chainLength = BlockchainObject.Instance.Chain.Count
            };

            return chainResponse;
        }


    }
}