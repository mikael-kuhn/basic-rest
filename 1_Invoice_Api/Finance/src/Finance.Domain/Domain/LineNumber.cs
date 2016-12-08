using System;

namespace Finance.Domain.Domain
{
    public sealed class LineNumber
    {
        public LineNumber(int value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException($"{value} must be a positive number");
            }

            Value = value;
        }

        public int Value { get; }
    }
}