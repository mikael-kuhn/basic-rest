using System;

namespace FinanceApiTest.Builders.Models
{
    using FinanceApi.Models;

    public sealed class GetInvoiceBuilder
    {
        public GetInvoice Build()
        {
            var invoiceDate = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);
            var dueDate = invoiceDate.AddDays(7);

            return new GetInvoice
            {
                Customer = new GetInvoiceCustomer
                {
                    Name = "Customer",
                    AddressLines = new [] { "Address"}
                },
                DueDate = dueDate,
                InvoiceDate = invoiceDate,
                SubTotal = 10,
                Id = "1",
                Lines = new [] { new GetInvoiceLine { Description = "Description",
                    LineNumber = 1, ItemPrice = 10, Quantity = 1, Total = 10}}
            };
        }
    }
}