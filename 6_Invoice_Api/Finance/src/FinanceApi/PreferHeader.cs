using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceApi
{
    public class PreferHeader
    {
        public PreferHeader(IEnumerable<string> values)
        {
            var allValues = values.SelectMany(value => value.Split(';')).Select(value =>
            {
                var nameValue = value.Split('=');
                var parsedValue = (nameValue.Length == 1) ? string.Empty : nameValue[1];
                return new NameValue(nameValue[0], parsedValue);
            });

            Return =
                allValues.FirstOrDefault(
                    nameValue => nameValue.Name.Equals("return",
                        StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;
        }

        public string Return { get; }

        private class NameValue
        {
            public NameValue(string name, string value)
            {
                Name = name;
                Value = value;
            }

            internal string Name { get; }
            internal string Value { get; }
        }
    }
}