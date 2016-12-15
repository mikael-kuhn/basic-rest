namespace Finance.Domain.Domain
{
    public struct Quantity
    {
        public Quantity(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; }
    }
}