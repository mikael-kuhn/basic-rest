using System;
using System.Collections.Generic;

namespace FinanceApi.Models
{
    public sealed class GetInvoice
    {
        public DateTimeOffset InvoiceDate { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public GetInvoiceCustomer Customer { get; set; }
        public decimal SubTotal { get; set; }
        public IEnumerable<GetInvoiceLine> Lines { get; set; }
        public string Id { get; set; }
    }
}