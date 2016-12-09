using System;
using System.Collections.Generic;

namespace FinanceApiTest.Builders.Domain
{
    using Finance.Domain.Domain;

    public sealed class InvoiceBuilder
    {
        private IEnumerable<InvoiceLine> lines = new[] {new InvoiceLineBuilder().Build()};

        public InvoiceBuilder WithLines(IEnumerable<InvoiceLine> lines)
        {
            this.lines = lines;
            return this;
        }

        public Invoice Build()
        {
            var invoiceDate = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);
            var dueDate = invoiceDate.AddDays(7);
            var customer = new InvoiceCustomer("Customer", new [] { "Address"});

            return new Invoice("1", invoiceDate, dueDate, customer, lines);
        }
    }
}