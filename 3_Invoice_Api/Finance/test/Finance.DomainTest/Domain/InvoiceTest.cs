using System;
using System.Linq;
using Xunit;

namespace Finance.DomainTest.Domain
{
    using Finance.Domain.Domain;

    public sealed class InvoiceTest
    {
        [Fact]
        public void Constructor_should_assign_InvoiceDate()
        {
            // Arrange
            var invoiceDate = new DateTimeOffset(2016, 12, 5, 0, 0, 0, TimeSpan.Zero);
            var dueDate = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);

            // Act
            var actual = new Invoice("1", Guid.Empty.ToString(), invoiceDate, dueDate, null, null).InvoiceDate;

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
            var actual = new Invoice("1", Guid.Empty.ToString(), invoiceDate, dueDate, null, null).DueDate;

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
            var actual = new Invoice("1", Guid.Empty.ToString(), invoiceDate, dueDate, customer, null).DueDate;

            // Assert
            Assert.Equal(dueDate, actual);
        }

        [Fact]
        public void Constuctor_should_assign_Lines()
        {
            // Arrange
            var invoiceDate = new DateTimeOffset(2016, 12, 5, 0, 0, 0, TimeSpan.Zero);
            var dueDate = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);
            var lines = new[] {new InvoiceLine(new LineNumber(1), new Quantity(1), new Amount(1), "description")};

            // Act
            var actual = new Invoice("1", Guid.Empty.ToString(), invoiceDate, dueDate, null, lines).Lines;

            // Assert
            Assert.Equal(1, actual.Count());
        }

        [Fact]
        public void SubTotal_should_be_the_sum_of_lines()
        {
            // Arrange
            var invoiceDate = new DateTimeOffset(2016, 12, 5, 0, 0, 0, TimeSpan.Zero);
            var dueDate = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);
            var firstLine = new InvoiceLine(new LineNumber(1), new Quantity(1), new Amount(1), "description");
            var secondLine = new InvoiceLine(new LineNumber(1), new Quantity(2), new Amount(2), "description");
            var lines = new[] { firstLine, secondLine};

            // Act
            var actual = new Invoice("1", Guid.Empty.ToString(), invoiceDate, dueDate, null, lines).SubTotal;

            // Assert
            Assert.Equal(firstLine.Total.Value + secondLine.Total.Value, actual.Value);
        }
    }
}
