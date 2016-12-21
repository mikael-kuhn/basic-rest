using System;
using FluentAssertions;
using Xunit;

namespace FinanceApiTest.Mappers
{
    using FinanceApi.Mappers;
    using Builders.Domain;
    using Builders.Models;

    public sealed class UpdateInvoiceMapperTest
    {
        private readonly UpdateInvoiceMapper testSubject;

        public UpdateInvoiceMapperTest()
        {
            testSubject = new UpdateInvoiceMapper();
        }

        [Fact]
        public void ToDomain()
        {
            // Arrange
            var updateInvoice = new UpdateInvoiceBuilder().Build();
            var expected = new InvoiceBuilder().Build();

            // Act
            var actual = testSubject.ToDomain(updateInvoice, expected.Id, Guid.Empty.ToString());

            // Assert
            actual.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void ToDomain_should_not_accept_null_model()
        {
            // Act
            var exception = Record.Exception(() => testSubject.ToDomain(null));

            // Assert
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ToModel()
        {
            // Arrange
            var invoice = new InvoiceBuilder().Build();
            var expected = new UpdateInvoiceBuilder().Build();

            // Act
            var actual = testSubject.ToModel(invoice);

            // Assert
            actual.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void ToModel_should_not_accept_null_model()
        {
            // Act
            var exception = Record.Exception(() => testSubject.ToDomain(null));

            // Assert
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}