using System;
using Xunit;
using FluentAssertions;

namespace FinanceApiTest.Mappers
{
    using FinanceApi.Mappers;
    using Builders.Domain;

    public sealed class GetInvoiceMapperTest
    {
        private readonly GetInvoiceMapper testSubject;

        public GetInvoiceMapperTest()
        {
            testSubject = new GetInvoiceMapper();
        }

        [Fact]
        public void ToModel()
        {
            // Arrange
            var domainInvoice = new InvoiceBuilder().Build();
            var expected = new Builders.Models.GetInvoiceBuilder().Build();

            // Act
            var actual = testSubject.ToModel(domainInvoice);

            // Assert
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void ToModel_should_not_accept_null()
        {
            // Act
            var exception = Record.Exception(() => testSubject.ToModel(null));

            // Assert
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}