using System;

namespace FinanceApiTest.Builders.Models
{
    using FinanceApi.Models;

    public sealed class GetMinimalInvoiceBuilder
    {
        public GetMinimalInvoice Build()
        {
            var invoiceDate = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);
            var dueDate = invoiceDate.AddDays(7);

            return new GetMinimalInvoice
            {
                Customer = new GetInvoiceCustomer
                {
                    Name = "Customer",
                    AddressLines = new [] { "Address"}
                },
                DueDate = dueDate,
                InvoiceDate = invoiceDate,
                SubTotal = 10,
                Id = "1"
            };
        }

    }
}