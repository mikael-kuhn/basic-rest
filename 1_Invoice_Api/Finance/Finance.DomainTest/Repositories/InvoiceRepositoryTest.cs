using Xunit;

namespace Finance.DomainTest.Repositories
{
    using Finance.Domain.Repositories;

    public sealed class InvoiceRepositoryTest
    {
        private readonly InvoiceRepository testSubject;

        public InvoiceRepositoryTest()
        {
            testSubject = new InvoiceRepository();
        }

        [Theory]
        public void Get_Should_Return_Invoice()
        {
            var actual = testSubject.Get("1234");
        }
    }
}