using Xunit;

namespace FinanceApiTest
{
    using FinanceApi;

    public sealed class PreferHeaderTest
    {
        [Theory]
        [InlineData(new [] { "wait", "return=representation; foo=12"}, "representation")]
        [InlineData(new [] { "wait", "return=minimal; foo=12"}, "minimal")]
        [InlineData(new [] { "wait", "foo=12;return=minimal"}, "minimal")]
        [InlineData(new [] { "wait", "return=minimal"}, "minimal")]
        [InlineData(new [] { "wait" }, "")]
        [InlineData(new [] { "return" }, "")]
        public void Return(string[] values, string expected)
        {
            // Act
            var actual = new PreferHeader(values).Return;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}