using System;

namespace FinanceApi.Models
{
    public class GetMinimalInvoice
    {
        public DateTimeOffset InvoiceDate { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public GetInvoiceCustomer Customer { get; set; }
        public decimal SubTotal { get; set; }
        public string Id { get; set; }
    }
}