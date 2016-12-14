using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinanceApi.Models
{
    public sealed class UpdateInvoiceCustomer
    {
        [Required(ErrorMessage = "Customer name must be specified")]
        [MaxLength(500, ErrorMessage = "Customer name is too long")]
        public string Name { get; set; }

        public IEnumerable<string> AddressLines { get; set; }
    }
}