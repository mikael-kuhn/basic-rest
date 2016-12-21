using System.Buffers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Net.Http.Headers;
using Xunit;
using Moq;
using System.Linq;

namespace FinanceApiTest
{
    using FinanceApi;

    public sealed class MvcOptionsExtensionsTest
    {
        private MvcOptions testSubject;

        public MvcOptionsExtensionsTest()
        {
            testSubject = new MvcOptions();

            var serializerSettings = JsonSerializerSettingsProvider.CreateSerializerSettings();

            testSubject.InputFormatters.Add(
                new JsonInputFormatter(new Mock<ILogger>().Object,
                serializerSettings,
                ArrayPool<char>.Shared,
                new Mock<ObjectPoolProvider>().Object));

            testSubject.OutputFormatters.Add(
                new JsonOutputFormatter(serializerSettings, ArrayPool<char>.Shared));
        }

        [Fact]
        public void SetInputMediaType_should_set_mediaType()
        {
            // Arrange
            var expected = new MediaTypeHeaderValue("application/vnd.test+json");

            // Act
            testSubject.SetInputMediaType(expected);

            var actual =
                testSubject.InputFormatters.OfType<JsonInputFormatter>().First().SupportedMediaTypes.First();

            // Assert
            Assert.Equal(expected.MediaType, actual);
        }
    }
}
