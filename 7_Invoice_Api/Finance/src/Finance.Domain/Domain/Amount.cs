namespace Finance.Domain.Domain
{
    public struct Amount
    {
        public Amount(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; }
    }
}