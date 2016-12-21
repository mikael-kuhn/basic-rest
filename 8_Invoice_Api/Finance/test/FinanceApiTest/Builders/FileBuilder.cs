using System;
using System.Text;

namespace FinanceApiTest.Builders
{
    using Finance.Domain.Domain;

    public sealed class FileBuilder
    {
        private DateTimeOffset lastModified = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);
        private bool isAvailable = true;

        public FileBuilder WithLastModified(DateTimeOffset lastModified)
        {
            this.lastModified = lastModified;
            return this;
        }

        public FileBuilder SetIsAvailable(bool isAvailable)
        {
            this.isAvailable = isAvailable;
            return this;
        }

        public File Build()
        {
            return new File(Encoding.UTF8.GetBytes("test"), lastModified,
                "text/plain") { IsAvailable = isAvailable};
        }
    }
}