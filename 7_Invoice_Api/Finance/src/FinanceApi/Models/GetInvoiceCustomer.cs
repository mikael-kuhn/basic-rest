using System.Collections.Generic;

namespace FinanceApi.Models
{
    public sealed class GetInvoiceCustomer
    {
        public string Name { get; set; }
        public IEnumerable<string> AddressLines { get; set; }
    }
}