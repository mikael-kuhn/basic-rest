using System;
using System.Collections.Generic;
using System.Linq;

namespace Finance.Domain.Domain
{
    public sealed class Invoice
    {
        public Invoice(string id, DateTimeOffset invoiceDate,
            DateTimeOffset dueDate,
            InvoiceCustomer invoiceCustomer,
            IEnumerable<InvoiceLine> lines)
        {
            InvoiceDate = invoiceDate;
            DueDate = dueDate;
            Customer = invoiceCustomer;
            Lines = lines;
            Id = id;
        }

        public string Id { get; set; }
        public DateTimeOffset InvoiceDate { get; }
        public DateTimeOffset DueDate { get; }
        public InvoiceCustomer Customer { get; }
        public IEnumerable<InvoiceLine> Lines { get; }
        public Amount SubTotal => new Amount(Lines.Sum(l => l.Total.Value));
    }
}