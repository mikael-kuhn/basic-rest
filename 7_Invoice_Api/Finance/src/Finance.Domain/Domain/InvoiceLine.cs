namespace Finance.Domain.Domain
{
    public sealed class InvoiceLine
    {
        public InvoiceLine(LineNumber lineNumber, Quantity quantity, Amount itemPrice, string description)
        {
            LineNumber = lineNumber;
            Quantity = quantity;
            ItemPrice = itemPrice;
            Description = description;
        }

        public LineNumber LineNumber { get; }
        public string Description { get; }
        public Quantity Quantity { get; }
        public Amount ItemPrice { get;  }
        public Amount Total => new Amount(Quantity.Value * ItemPrice.Value);
    }
}