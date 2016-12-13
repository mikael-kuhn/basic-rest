using System;
using System.Collections.Generic;
using Xunit;

namespace Finance.DomainTest.Repositories
{
    using Finance.Domain.Repositories;
    using Finance.Domain.Domain;

    public sealed class InvoiceRepositoryTest
    {
        private readonly InvoiceRepository testSubject;

        public InvoiceRepositoryTest()
        {
            testSubject = new InvoiceRepository();
        }

        [Fact]
        public void Get_should_return_invoice()
        {
            // Hardcoded invoice
            var actual = testSubject.Get("1");

            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void Get_should_not_return_invoice()
        {
            // Hardcoded invoice
            var actual = testSubject.Get("x");

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void Update_should_replace_invoice()
        {
            // Arrange
            var expected = new Invoice("1000", Guid.Empty.ToString(),  DateTimeOffset.Now.Date, DateTimeOffset.Now.Date,
                new InvoiceCustomer("Customer 1000", new List<string>()), new List<InvoiceLine>());

            // Act - yeah this is not pretty
            testSubject.Update(expected);
            var actual = testSubject.Get(expected.Id);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_should_fail_when_invoice_does_not_exist()
        {
            // Arrange
            var expected = new Invoice("unknown", Guid.Empty.ToString(), DateTimeOffset.Now.Date, DateTimeOffset.Now.Date,
                new InvoiceCustomer("Customer 1001", new List<string>()), new List<InvoiceLine>());

            // Act
            var exception = Record.Exception(() => testSubject.Update(expected));

            // Assert
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void Create_should_return_an_invoice()
        {
            // Arrange
            var instance = new Invoice("1001", Guid.Empty.ToString(), DateTimeOffset.Now.Date, DateTimeOffset.Now.Date,
                new InvoiceCustomer("Customer 1001", new List<string>()), new List<InvoiceLine>());

            // Act
            var response = testSubject.Create(instance);

            // Assert
            Assert.NotNull(response);
        }

        [Fact]
        public void Exists_should_be_true_when_invoice_exists()
        {
            // Act and Assert
            Assert.True(testSubject.Exists("1000"));
        }

        [Fact]
        public void Exists_should_be_false_when_invoice_does_not_exist()
        {
            // Act and Assert
            Assert.False(testSubject.Exists("unknown"));
        }
    }
}