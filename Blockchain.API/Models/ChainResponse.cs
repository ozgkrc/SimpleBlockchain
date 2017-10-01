using System.Collections.Generic;

namespace Blockchain.API.Controllers
{
    public class ChainResponse
    {
        public List<Lib.Block> chain { get; set; }
        public int chainLength { get; set; }
    }
}