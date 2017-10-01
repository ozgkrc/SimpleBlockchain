using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blockchain.API
{
    public class BlockchainObject
    {
        private static readonly Lib.Blockchain _chain = new Lib.Blockchain();
        public static Lib.Blockchain Instance
        {
            get
            {
                return _chain;
            }
        }
    }
}
