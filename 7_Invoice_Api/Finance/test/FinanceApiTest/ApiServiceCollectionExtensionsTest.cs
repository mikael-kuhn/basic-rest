using System;
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

        [Theory]
        [InlineData(typeof(GetInvoiceMapper), typeof(IModelDomainMapper<GetInvoice, Domain.Invoice>))]
        [InlineData(typeof(UpdateInvoiceMapper), typeof(IModelDomainMapper<UpdateInvoice, Domain.Invoice>))]
        [InlineData(typeof(GetMinimalInvoiceMapper), typeof(IModelDomainMapper<GetMinimalInvoice, Domain.Invoice>))]
        public void AddApi(Type implementationType, Type serviceType)
        {
            // Act
            serviceCollectionMock.Object.AddApi();

            // Assert
            serviceCollectionMock.Verify(sc =>
                sc.Add(
                    It.Is<ServiceDescriptor>(
                        sd =>
                            sd.ImplementationType == implementationType &&
                            sd.ServiceType == serviceType &&
                            sd.Lifetime == ServiceLifetime.Transient)));
        }
    }
}