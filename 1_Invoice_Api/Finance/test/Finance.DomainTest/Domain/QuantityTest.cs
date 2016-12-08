using Xunit;

namespace Finance.DomainTest.Domain
{
    using Finance.Domain.Domain;

    public sealed class QuantityTest
    {
        [Fact]
        public void Constructor_should_assign_Value()
        {
            // Arrange
            const decimal expected = 1.5m;

            // Act
            var actual = new Quantity(expected).Value;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}