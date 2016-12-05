using System;
using System.Collections.Generic;

namespace Finance.Domain.Domain
{
    public sealed class Invoice
    {
        public Invoice(Guid id, DateTimeOffset invoiceDate,
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

        public Guid Id { get; }
        public DateTimeOffset InvoiceDate { get; }
        public DateTimeOffset DueDate { get; }
        public InvoiceCustomer Customer { get; }
        public IEnumerable<InvoiceLine> Lines { get; }
    }
}