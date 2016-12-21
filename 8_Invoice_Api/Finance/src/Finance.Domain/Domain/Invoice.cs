using System;
using System.Collections.Generic;
using System.Linq;

namespace Finance.Domain.Domain
{
    public sealed class Invoice
    {
        public Invoice(string id,
            string version,
            DateTimeOffset invoiceDate,
            DateTimeOffset dueDate,
            InvoiceCustomer invoiceCustomer,
            IEnumerable<InvoiceLine> lines)
        {
            InvoiceDate = invoiceDate;
            DueDate = dueDate;
            Customer = invoiceCustomer;
            Lines = lines;
            Id = id;
            Version = version;
        }

        public string Id { get; set; }
        public string Version { get; set; }
        public DateTimeOffset InvoiceDate { get; }
        public DateTimeOffset DueDate { get; }
        public InvoiceCustomer Customer { get; }
        public IEnumerable<InvoiceLine> Lines { get; }
        public Amount SubTotal => new Amount(Lines.Sum(l => l.Total.Value));
    }
}