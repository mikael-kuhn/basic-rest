using System;
using System.Collections.Generic;

namespace FinanceApiTest.Builders.Models
{
    using FinanceApi.Models;

    public sealed class UpdateInvoiceBuilder
    {
        private UpdateInvoiceCustomer customer = new UpdateInvoiceCustomer
        {
            Name = "Customer",
            AddressLines = new[] {"Address"}
        };

        private IEnumerable<UpdateInvoiceLine> lines = new[]
        {
            new UpdateInvoiceLine
            {
                Description = "Description",
                LineNumber = 1,
                ItemPrice = 10,
                Quantity = 1
            }
        };

        public UpdateInvoiceBuilder WithCustomer(UpdateInvoiceCustomer customer)
        {
            this.customer = customer;
            return this;
        }

        public UpdateInvoiceBuilder WithLines(IEnumerable<UpdateInvoiceLine> lines)
        {
            this.lines = lines;
            return this;
        }

        public UpdateInvoice Build()
        {
            var invoiceDate = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);
            var dueDate = invoiceDate.AddDays(7);

            return new UpdateInvoice
            {
                Customer = customer,
                DueDate = dueDate,
                InvoiceDate = invoiceDate,
                Lines = lines
            };

        }
    }
}
