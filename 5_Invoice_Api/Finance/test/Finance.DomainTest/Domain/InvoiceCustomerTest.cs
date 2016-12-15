using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Finance.DomainTest.Domain
{
    using Finance.Domain.Domain;

    public sealed class InvoiceCustomerTest
    {
        [Fact]
        public void Constructor_should_assign_Name()
        {
            // Arrange
            const string expected = "name";

            // Act
            var actual = new InvoiceCustomer(expected, new List<string>()).Name;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Constructor_addressLines_must_be_specified()
        {
            // Act
            var exception = Record.Exception(() => new InvoiceCustomer("name", null));

            // Assert
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Constructor_should_assign_immutable_AddressLines()
        {
            // Arrange
            var addressLines = new List<string> { "line 1"};

            // Act
            var actual = new InvoiceCustomer("name", addressLines);

            // Assert
            Assert.Equal(1, actual.AddressLines.Count());

            addressLines.Add("line 2");

            Assert.Equal(1, actual.AddressLines.Count());
        }
    }
}