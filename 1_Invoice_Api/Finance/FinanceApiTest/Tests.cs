using Xunit;
using FinanceApi.Controllers;

namespace FinanceApiTest
{
    public class Tests
    {
        [Fact]
        public void Test1()
        {
            var t = new InvoiceController();
            Assert.Equal("value", t.Get(""));
        }
    }
}