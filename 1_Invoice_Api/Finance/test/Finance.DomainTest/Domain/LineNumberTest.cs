using System;
using Xunit;

namespace Finance.DomainTest.Domain
{
    using Finance.Domain.Domain;

    public sealed class LineNumberTest
    {
        [Fact]
        public void Constructor_should_assign_positive_value()
        {
            // Arrange
            const int expected = 2;

            // Act
            var actual = new LineNumber(expected).Value;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Constructor_should_not_assign_negative_value()
        {
            // Act
            var exception = Record.Exception(() => new LineNumber(-1));

            // Assert
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }
    }
}