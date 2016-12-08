using Finance.Domain.Domain;

namespace FinanceApiTest.Builders.Domain
{
    public sealed class InvoiceLineBuilder
    {
        public InvoiceLine Build()
        {
            return new InvoiceLine(new LineNumber(1), new Quantity(1), new Amount(10), "Description" );
        }
    }
}