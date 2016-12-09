using Finance.Domain.Domain;
using Finance.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Domain
{
    public static class DomainServiceCollectionExtensions
    {
        public static void AddDomain(this IServiceCollection services)
        {
            services.AddSingleton<IRepository<Invoice>, InvoiceRepository>();
        }
    }
}
