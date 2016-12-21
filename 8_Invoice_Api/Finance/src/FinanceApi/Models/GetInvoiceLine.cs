namespace FinanceApi.Models
{
    public sealed class GetInvoiceLine
    {
        public int LineNumber { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal Total { get; set; }
    }
}