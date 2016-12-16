using FinanceApi;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace FinanceApiTest
{
    public sealed class HeaderDictionaryExtensionsTest
    {
        private readonly HeaderDictionary testSubject = new HeaderDictionary();

        [Fact]
        public void Prefer_should_return_instance()
        {
            // Arrange
            testSubject.Add("Prefer", new [] { "wait", "return=representation; foo=12"});

            // Act
            var response = testSubject.Prefer();

            // Assert
            Assert.NotNull(response);
        }

        [Fact]
        public void Prefer_should__return_instance_when_header_is_not_there()
        {
            // Act
            var response = testSubject.Prefer();

            // Assert
            Assert.NotNull(response);
        }
    }
}