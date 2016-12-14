using System;

namespace FinanceApi.Mappers
{
    using Models;
    using Domain = Finance.Domain.Domain;

    public sealed class GetMinimalInvoiceMapper : IModelDomainMapper<GetMinimalInvoice, Domain.Invoice>
    {
        public GetMinimalInvoice ToModel(Domain.Invoice domainInstance)
        {
            if (domainInstance == null)
            {
                throw new ArgumentNullException(nameof(domainInstance));
            }

            var invoice =  new GetMinimalInvoice
            {
                Id = domainInstance.Id,
                InvoiceDate = domainInstance.InvoiceDate,
                DueDate = domainInstance.DueDate,
                SubTotal = domainInstance.SubTotal.Value,
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

        public Domain.Invoice ToDomain(GetMinimalInvoice modelInstance, string id = null, string version = null)
        {
            throw new System.NotImplementedException();
        }
    }
}