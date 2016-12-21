using Xunit;

namespace Finance.DomainTest.Domain
{
    using Finance.Domain.Domain;

    public sealed class InvoiceLineTest
    {
        [Fact]
        public void Constructor_should_assign_properties()
        {
            // Arrange
            var lineNumber = new LineNumber(1);
            var quantity = new Quantity(1);
            var itemPrice = new Amount(10m);
            const string description = "description";

            // Act
            var actual = new InvoiceLine(lineNumber,
                quantity,
                itemPrice,
                description);

            // Assert
            Assert.Equal(lineNumber, actual.LineNumber);
            Assert.Equal(quantity, actual.Quantity);
            Assert.Equal(itemPrice, actual.ItemPrice);
            Assert.Equal(description, actual.Description);
        }

        [Fact]
        public void Total_should_be_the_product_of_ItemPrice_and_Quantity()
        {
            // Arrange
            var quantity = new Quantity(2);
            var itemPrice = new Amount(2);

            var line = new InvoiceLine(new LineNumber(1), quantity, itemPrice, "Pencil");

            // Act and Assert
            Assert.Equal(quantity.Value * itemPrice.Value, line.Total.Value);
        }
    }
}
