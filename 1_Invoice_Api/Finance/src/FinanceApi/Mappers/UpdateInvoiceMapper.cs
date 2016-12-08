using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceApi.Mappers
{
    using Models;
    using Domain = Finance.Domain.Domain;

    public sealed class UpdateInvoiceMapper : IModelDomainMapper<UpdateInvoice, Domain.Invoice>
    {
        public UpdateInvoice ToModel(Domain.Invoice domainInstance)
        {
            throw new NotImplementedException();
        }

        public Domain.Invoice ToDomain(UpdateInvoice modelInstance, string id = null)
        {
            if (modelInstance == null)
            {
                throw new ArgumentNullException(nameof(modelInstance));
            }

            return new Domain.Invoice(id,
                modelInstance.InvoiceDate,
                modelInstance.DueDate, 
                GetDomainCustomerFrom(modelInstance.Customer), 
                GetDomainInvoiceLinesFrom(modelInstance.Lines));
        }

        private static IEnumerable<Domain.InvoiceLine> GetDomainInvoiceLinesFrom(
            IEnumerable<UpdateInvoiceLine> modelInstanceLines)
        {
            return modelInstanceLines.Select(line => new Domain.InvoiceLine(
                new Domain.LineNumber(line.LineNumber),
                new Domain.Quantity(line.Quantity),
                new Domain.Amount(line.ItemPrice),
                line.Description
            ));
        }

        private static Domain.InvoiceCustomer GetDomainCustomerFrom(UpdateInvoiceCustomer modelInstanceCustomer)
        {
            if (modelInstanceCustomer == null)
            {
                return null;
            }
            return new Domain.InvoiceCustomer(modelInstanceCustomer.Name, modelInstanceCustomer.AddressLines);
        }
    }
}