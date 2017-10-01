using System.ComponentModel.DataAnnotations;

namespace Blockchain.API
{
    public class TransactionRequest
    {
        [Required]
        public string Sender { get; set; }
        [Required]
        public string Recipient { get; set; }
        [Required]
        public double Amount { get; set; }
    }
}