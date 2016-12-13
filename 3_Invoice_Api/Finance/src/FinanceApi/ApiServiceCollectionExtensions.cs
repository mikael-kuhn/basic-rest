using Microsoft.Extensions.DependencyInjection;

namespace FinanceApi
{
    using Mappers;
    using Domain = Finance.Domain.Domain;

    public static class ApiServiceCollectionExtensions
    {
        public static void AddApi(this IServiceCollection services)
        {
            services.AddTransient<IModelDomainMapper<Models.GetInvoice, Domain.Invoice>, GetInvoiceMapper>();
            services.AddTransient<IModelDomainMapper<Models.UpdateInvoice, Domain.Invoice>, UpdateInvoiceMapper>();
        }

    }
}