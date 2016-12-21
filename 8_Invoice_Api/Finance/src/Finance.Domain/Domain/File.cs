using System;

namespace Finance.Domain.Domain
{
    public sealed class File
    {
        public File(byte[] content, DateTimeOffset lastModified, string contentType)
        {
            Content = content;
            LastModified = lastModified;
            ContentType = contentType;
        }

        public byte[] Content { get; }
        public DateTimeOffset LastModified { get; }
        public string ContentType { get; }
        public string Id { get; set; }
        public bool IsAvailable = true;
    }
}