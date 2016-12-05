using System.Collections.Generic;

namespace Finance.Domain.Domain
{
    public class InvoiceCustomer
    {
        public InvoiceCustomer(string name, IEnumerable<string> addressLines)
        {
            Name = name;
            AddressLines = addressLines;
        }

        public string Name { get; }
        public IEnumerable<string> AddressLines { get; }
    }
}