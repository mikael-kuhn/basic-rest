using System;
using System.Linq;

namespace FinanceApi.Mappers
{
    using Models;
    using Domain = Finance.Domain.Domain;

    public sealed class GetInvoiceMapper : IModelDomainMapper<GetInvoice, Domain.Invoice>
    {
        public GetInvoice ToModel(Domain.Invoice domainInstance)
        {
            if (domainInstance == null)
            {
                throw new ArgumentNullException(nameof(domainInstance));
            }

            var invoice =  new GetInvoice
            {
                Id = domainInstance.Id,
                InvoiceDate = domainInstance.InvoiceDate,
                DueDate = domainInstance.DueDate,
                SubTotal = domainInstance.SubTotal.Value,
                Lines = domainInstance.Lines.Select(line => new GetInvoiceLine
                {
                    Description = line.Description,
                    ItemPrice = line.ItemPrice.Value,
                    LineNumber = line.LineNumber.Value,
                    Quantity = line.Quantity.Value,
                    Total = line.Total.Value
                } )
            };

            if (domainInstance.Customer != null)
            {
                invoice.Customer = new GetInvoiceCustomer
                {
                    Name = domainInstance.Customer.Name,
                    AddressLines =  domainInstance.Customer.AddressLines
                };
            }

            return invoice;
        }

        public Domain.Invoice ToDomain(GetInvoice modelInstance, string id = null, string version = null)
        {
            throw new System.NotImplementedException();
        }
    }
}