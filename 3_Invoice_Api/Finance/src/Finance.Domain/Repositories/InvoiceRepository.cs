using System;
using System.Collections.Generic;
using System.Linq;

namespace Finance.Domain.Repositories
{
    using Domain;

    public sealed class InvoiceRepository : IRepository<Invoice>
    {
        private readonly Dictionary<string, Invoice> invoices = Enumerable.Range(1, 1000).Select(index =>
                    Tuple.Create(index.ToString(),
                        new Invoice(index.ToString(), Guid.NewGuid().ToString(),
                            DateTimeOffset.Now.Date, DateTimeOffset.Now.Date,
                            new InvoiceCustomer($"Customer {index}", new List<string>()),
                            new [] { new InvoiceLine(new LineNumber(1), new Quantity(2), new Amount(10), "Line 1") }
                        )))
                .ToDictionary(t => t.Item1, t => t.Item2);

        public Invoice Get(string id)
        {

            if (!invoices.ContainsKey(id))
            {
                return null;
            }

            return invoices[id];
        }

        public bool Exists(string id)
        {
            return invoices.ContainsKey(id);
        }

        public string GetCurrentVersion(string id)
        {
            return invoices[id]?.Version;
        }

        public Version Update(Invoice instance)
        {
            if (!invoices.ContainsKey(instance.Id))
            {
                throw new InvalidOperationException($"Cannot update {instance.Id}. It does not exist");
            }

            instance.Version = Guid.NewGuid().ToString();
            invoices[instance.Id] = instance;

            return new Version("x");
        }

        public Invoice Create(Invoice instance)
        {
            // This is an ugly hack - we will need to change it
            instance.Id = (invoices.Keys.Select(int.Parse).Max() + 1).ToString();
            instance.Version = Guid.NewGuid().ToString();

            invoices.Add(instance.Id, instance);
            return instance;
        }
    }
}