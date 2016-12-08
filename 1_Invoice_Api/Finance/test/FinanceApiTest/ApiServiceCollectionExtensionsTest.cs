using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace FinanceApiTest
{
    using FinanceApi.Mappers;
    using FinanceApi;
    using FinanceApi.Models;
    using Domain = Finance.Domain.Domain;

    public sealed class ApiServiceCollectionExtensionsTest
    {
        private readonly Mock<IServiceCollection> serviceCollectionMock;

        public ApiServiceCollectionExtensionsTest()
        {
            serviceCollectionMock = new Mock<IServiceCollection>();
        }

        [Fact]
        public void AddApi_GetInvoiceMapper_should_be_added()
        {
            // Act
            serviceCollectionMock.Object.AddApi();

            // Assert
            serviceCollectionMock.Verify(sc =>
                sc.Add(
                    It.Is<ServiceDescriptor>(
                        sd =>
                            sd.ImplementationType == typeof(GetInvoiceMapper) &&
                            sd.ServiceType == typeof(IModelDomainMapper<GetInvoice, Domain.Invoice>) &&
                            sd.Lifetime == ServiceLifetime.Transient)));
        }

        [Fact]
        public void AddApi_UpdateInvoiceMapper_should_be_added()
        {
            // Act
            serviceCollectionMock.Object.AddApi();

            // Assert
            serviceCollectionMock.Verify(sc =>
                sc.Add(
                    It.Is<ServiceDescriptor>(
                        sd =>
                            sd.ImplementationType == typeof(UpdateInvoiceMapper) &&
                            sd.ServiceType == typeof(IModelDomainMapper<UpdateInvoice, Domain.Invoice>) &&
                            sd.Lifetime == ServiceLifetime.Transient)));
        }
    }
}