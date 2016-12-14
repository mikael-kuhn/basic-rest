using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Linq;

namespace FinanceApi
{
    public static class MvcOptionsExtensions
    {
        public static void SetInputMediaType(this MvcOptions options, MediaTypeHeaderValue mediaType)
        {
            var supportedInputMediaTypes = options
                .InputFormatters
                .OfType<JsonInputFormatter>()
                .First()
                .SupportedMediaTypes;

            SetAllowedMediaType(mediaType, supportedInputMediaTypes);
        }

        public static void SetOutputMediaType(this MvcOptions options, MediaTypeHeaderValue mediaType)
        {
            var supportedOutputMediaTypes = options
                .OutputFormatters
                .OfType<JsonOutputFormatter>()
                .First()
                .SupportedMediaTypes;

            SetAllowedMediaType(mediaType, supportedOutputMediaTypes);
        }

        private static void SetAllowedMediaType(MediaTypeHeaderValue mediaType,
            MediaTypeCollection supportedMediaTypes)
        {
            supportedMediaTypes.Clear();
            supportedMediaTypes.Add(mediaType);
        }
    }
}