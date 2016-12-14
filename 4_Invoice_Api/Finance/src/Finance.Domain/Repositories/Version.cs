namespace Finance.Domain.Repositories
{
    public sealed class Version
    {
        public Version(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}