using Xunit;

namespace Finance.DomainTest.Domain
{
    using Finance.Domain.Domain;

    public sealed class AmountTest
    {
        [Fact]
        public void Constructor_should_assign_Value()
        {
            // Arrange
            const decimal expected = 10.5m;

            // Act
            var actual = new Amount(expected).Value;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}