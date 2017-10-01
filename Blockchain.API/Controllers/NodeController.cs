using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;

namespace Blockchain.API.Controllers
{
    [Produces("application/json")]
    public class NodeController : Controller
    {
        [Route("/nodes/register")]
        [HttpPost]
        public ActionResult Register(List<string> urls)
        {
            foreach (var url in urls)
            {
                var uri = new Uri(url);
                NodeRepository.Addresses.Add(uri);
            }

            return Content(urls.Count + " new nodes are registered.");
        }

        [Route("/nodes/resolve")]
        [HttpGet]
        public async Task<ContentResult> Consensus()
        {
            List<List<Blockchain.Lib.Block>> chains = new List<List<Lib.Block>>();
            var client = new HttpClient();

            foreach (var nodeAddress in NodeRepository.Addresses)
            {
                var getTask = client.GetStringAsync(nodeAddress + "/chain");
                var chainResponse = await getTask;
                var nodeChain = JsonConvert.DeserializeObject<ChainResponse>(chainResponse);
                chains.Add(nodeChain.chain);
            }

            var result = BlockchainObject.Instance.ResolveConflict(chains);

            if (result)
                return Task.FromResult(Content("Conflict Resolved")).Result;
            else
                return Task.FromResult(Content("No need conflict detected")).Result;
        }
    }
}