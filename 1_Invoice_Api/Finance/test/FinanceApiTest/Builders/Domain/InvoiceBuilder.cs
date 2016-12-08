using System;

namespace FinanceApiTest.Builders.Domain
{
    using Finance.Domain.Domain;

    public sealed class InvoiceBuilder
    {
        public Invoice Build()
        {
            var invoiceDate = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);
            var dueDate = invoiceDate.AddDays(7);
            var customer = new InvoiceCustomer("Customer", new [] { "Address"});
            var lines = new[] {new InvoiceLineBuilder().Build()};

            return new Invoice("1", invoiceDate, dueDate, customer, lines);
        }
    }
}