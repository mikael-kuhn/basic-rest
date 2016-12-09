using System;
using System.Collections.Generic;

namespace FinanceApi.Models
{
    public sealed class UpdateInvoice
    {
        public DateTimeOffset InvoiceDate { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public UpdateInvoiceCustomer Customer { get; set; }
        public IEnumerable<UpdateInvoiceLine> Lines { get; set; }
    }
}