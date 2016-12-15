using System;
using System.Text;

namespace FinanceApiTest.Builders
{
    using Finance.Domain.Domain;

    public sealed class FileBuilder
    {
        private DateTimeOffset lastModified = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);

        public FileBuilder WithLastModified(DateTimeOffset lastModified)
        {
            this.lastModified = lastModified;
            return this;
        }

        public File Build()
        {
            return new File(Encoding.UTF8.GetBytes("test"), lastModified,
                "text/plain");
        }
    }
}