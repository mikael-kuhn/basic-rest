using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace FinanceApi
{
    public static class HeaderDictionaryExtensions
    {
        public const string PreferHeader = "Prefer";

        public static PreferHeader Prefer(this IHeaderDictionary headers)
        {
            return new PreferHeader(headers.ContainsKey(PreferHeader) ?
                (IEnumerable<string>) headers[PreferHeader] : new string[0]);
        }
    }
}