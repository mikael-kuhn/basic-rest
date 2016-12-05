namespace Finance.Domain.Domain
{
    public sealed class InvoiceLine
    {
        public InvoiceLine(LineNumber lineNumber, Quantity quantity, Amount itemPrice, string description)
        {

            Quantity = quantity;
            ItemPrice = itemPrice;
            Description = description;
        }

        public int LineNumber { get; }
        public string Description { get; }
        public Quantity Quantity { get; }
        public Amount ItemPrice { get;  }
    }
}