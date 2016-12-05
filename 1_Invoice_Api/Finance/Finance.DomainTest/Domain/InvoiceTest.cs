using System;
using Finance.Domain.Domain;
using Xunit;

namespace Finance.DomainTest.Domain
{
    public sealed class InvoiceTest
    {
        [Fact]
        public void Constructor_should_assign_InvoiceDate()
        {
            // Arrange
            var invoiceDate = new DateTimeOffset(2016, 12, 5, 0, 0, 0, TimeSpan.Zero);
            var dueDate = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);

            // Act
            var actual = new Invoice(Guid.NewGuid(), invoiceDate, dueDate, null, null).InvoiceDate;

            // Assert
            Assert.Equal(invoiceDate, actual);
        }

        [Fact]
        public void Constructor_should_assign_DueDate()
        {
            // Arrange
            var invoiceDate = new DateTimeOffset(2016, 12, 5, 0, 0, 0, TimeSpan.Zero);
            var dueDate = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);

            // Act
            var actual = new Invoice(Guid.NewGuid(), invoiceDate, dueDate, null, null).DueDate;

            // Assert
            Assert.Equal(dueDate, actual);
        }


        [Fact]
        public void Constuctor_should_assign_InvoiceCustomer()
        {
            // Arrange
            var invoiceDate = new DateTimeOffset(2016, 12, 5, 0, 0, 0, TimeSpan.Zero);
            var dueDate = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);
            var customer = new InvoiceCustomer("Microsoft", new [] { "Falkoner Alle"});

            // Act
            var actual = new Invoice(Guid.NewGuid(), invoiceDate, dueDate, customer, null).DueDate;

            // Assert
            Assert.Equal(dueDate, actual);
        }
    }
}
