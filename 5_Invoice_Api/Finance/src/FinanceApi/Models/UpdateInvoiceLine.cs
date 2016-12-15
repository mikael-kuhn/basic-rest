using System.ComponentModel.DataAnnotations;

namespace FinanceApi.Models
{
    public sealed class UpdateInvoiceLine
    {
        public int LineNumber { get; set; }
        public decimal Quantity { get; set; }
        public decimal ItemPrice { get; set; }

        [Required(ErrorMessage = "Invoice line description must be specified")]
        [MaxLength(500, ErrorMessage = "Invoice line description is too long")]
        public string Description { get; set; }
    }
}