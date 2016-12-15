using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Finance.DomainTest
{
    using Finance.Domain.Domain;
    using Finance.Domain.Repositories;
    using Finance.Domain;

    public sealed class DomainServiceCollectionExtensionsTest
    {
        private readonly Mock<IServiceCollection> serviceCollectionMock;

        public DomainServiceCollectionExtensionsTest()
        {
            serviceCollectionMock = new Mock<IServiceCollection>();
        }

        [Theory]
        [InlineData(typeof(InvoiceRepository), typeof(IRepository<Invoice>))]
        [InlineData(typeof(FileRepository), typeof(IRepository<File>))]
        public void AddDomain_(Type implementationType, Type serviceType)
        {
            // Act
            serviceCollectionMock.Object.AddDomain();

            // Assert
            serviceCollectionMock.Verify(sc =>
                sc.Add(
                    It.Is<ServiceDescriptor>(
                        sd =>
                            sd.ImplementationType == implementationType &&
                            sd.ServiceType == serviceType &&
                            sd.Lifetime == ServiceLifetime.Singleton)));
        }
    }
}